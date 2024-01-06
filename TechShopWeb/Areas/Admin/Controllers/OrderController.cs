using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopTech.DataAccess.Repository.IRepository;
using ShopTech.Models;
using ShopTech.Models.VModels;
using ShopTech.Utility;
using System.Security.Claims;

namespace TechShopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM orderVM { get; set; }
        public OrderController(IUnitOfWork unitofwork)
        {
            _unitOfWork = unitofwork;
        }
        public IActionResult Index()
        {
            IEnumerable<OrderHeader> orders;
            if (User.IsInRole(SD.Role_Admin))
            {
                orders = _unitOfWork._OrderHeaderRepository.GetAll("OrderHeaders",IncludePr: "AppUser");
            }
            else
            {
                var user = (ClaimsIdentity)User.Identity;
                var result = user.FindFirst(ClaimTypes.NameIdentifier).Value;

                orders = _unitOfWork._OrderHeaderRepository.GetAll("OrderHeaders",$"Where UserId = '{result}'", IncludePr: "AppUser");
            }
            return View(orders);
        }
        public IActionResult IndexSpecific(string status)
        {
            IEnumerable<OrderHeader> Orders;
            if (User.IsInRole(SD.Role_Admin))
            {
                Orders = _unitOfWork._OrderHeaderRepository.GetAll("OrderHeaders",$"Where OrderStatus ='{status}' Or PaymentStatus = '{status}'", IncludePr:"AppUser");
            }
            else
            {
                var user = (ClaimsIdentity)User.Identity;
                var result = user.FindFirst(ClaimTypes.NameIdentifier).Value;

                Orders = _unitOfWork._OrderHeaderRepository.GetAll("OrderHeaders", $"Where UserId = '{result}' and  (OrderStatus ='{status}' Or PaymentStatus = '{status}')", IncludePr: "AppUser");
            }
            return View(nameof(Index), Orders);
        }
        public IActionResult Details(int id)
        {
            orderVM = new OrderVM
            {
                OrderHeader = _unitOfWork._OrderHeaderRepository.GetOne("OrderHeaders", $"Where Id = {id}", IncludePr: "AppUser,Shipper"),
                OrderDetails = _unitOfWork._OrderDetailRepository.GetAll("OrderDetails", $"Where OrderId = {id}", IncludePr: "Product"),
                ListShippers = _unitOfWork._ShipperRepository.GetAll("Shippers").Select(
                       x => new SelectListItem
                       {
                           Text = x.CompanyName,
                           Value = x.Id.ToString()
                       }
                    )
            };
            return View(orderVM);
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult UpdateOrder()
        {
            var orderfromDB = _unitOfWork._OrderHeaderRepository.GetOne("OrderHeaders",$"Where Id = {orderVM.OrderHeader.Id}");
            orderfromDB.Name = orderVM.OrderHeader.Name;
            orderfromDB.PhoneNumber = orderVM.OrderHeader.PhoneNumber;
            orderfromDB.Adress = orderVM.OrderHeader.Adress;
            orderfromDB.City =orderVM.OrderHeader.City;
            orderfromDB.Email = orderVM.OrderHeader.Email;
            orderfromDB.PostalCode = orderVM.OrderHeader.PostalCode;
            if (orderVM.OrderHeader.ShipperId != null)
            {
                orderfromDB.ShipperId = orderVM.OrderHeader.ShipperId;
            }
            if (!string.IsNullOrEmpty(orderVM.OrderHeader.TrackingNumber))
            {
                orderfromDB.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
            }
            _unitOfWork._OrderHeaderRepository.Update(orderfromDB);
            TempData["success"] = "Order Details Updated Successfuly";
            return RedirectToAction(nameof(Details), new { id = orderfromDB.Id });
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Processing()
        {
            _unitOfWork._OrderHeaderRepository.UpdateStatus(orderVM.OrderHeader.Id, SD.StatusInProcess);
            TempData["success"] = "Order Is In Process";
            return RedirectToAction(nameof(Details), new { id = orderVM.OrderHeader.Id });
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Shipping()
        {
            OrderHeader ordhDB = _unitOfWork._OrderHeaderRepository.GetOne("OrderHeaders", $"Where Id = {orderVM.OrderHeader.Id}");
            ordhDB.OrderStatus = SD.StatusShipped;
            ordhDB.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
            ordhDB.ShipperId = orderVM.OrderHeader.ShipperId;
            ordhDB.OrderShippingDate = DateOnly.FromDateTime(DateTime.Now);

            _unitOfWork._OrderHeaderRepository.Update(ordhDB);

            TempData["success"] = "Order Is Shiped";
            return RedirectToAction(nameof(Details), new { id = orderVM.OrderHeader.Id });
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CancelOrder()
        {
            OrderHeader ordhDB = _unitOfWork._OrderHeaderRepository.GetOne("OrderHeaders", $"Where id = {orderVM.OrderHeader.Id}");
            if (ordhDB.OrderStatus == SD.StatusAproved)
            {
                // in here we should refund the payment
                _unitOfWork._OrderHeaderRepository.UpdateStatus(ordhDB.Id, SD.StatusCanceled, SD.StatusRefunded);
            }
            else
            {
                _unitOfWork._OrderHeaderRepository.UpdateStatus(ordhDB.Id, SD.StatusCanceled, SD.StatusCanceled);

            }
            TempData["success"] = "Order Is Cancelled";
            return RedirectToAction(nameof(Details), new { id = orderVM.OrderHeader.Id });
        }
    }
}
