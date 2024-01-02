using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopTech.DataAccess.Repository.IRepository;
using ShopTech.Models;
using ShopTech.Models.VModels;
using ShopTech.Utility;
using System.Security.Claims;

namespace TechShopWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShopingCartVM ShopCartVM { get; set; }
        public ShoppingCartController(IUnitOfWork unitofwork)
        {
            _unitOfWork = unitofwork;
        }
        [Authorize]
        public IActionResult Index()
        {
            var user = (ClaimsIdentity)User.Identity;
            string result = user.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShopCartVM = new ShopingCartVM()
            {
                ShoppingCarts = _unitOfWork._ShoppingCartRepository.GetAll("ShoppingCarts", 
                $"Where UserId = '{result}'", IncludePr: "Product"),
                OrderHeader = new()
            };
            decimal totalPrice = 0;
            foreach (var cart in ShopCartVM.ShoppingCarts)
            {
                totalPrice += cart.Price * cart.Quantity;
            }
            ShopCartVM.OrderHeader.OrderTotal = totalPrice;

            ShopCartVM.OrderHeader.AppUser = _unitOfWork.AppDbContext().ApplicationUsers.FirstOrDefault(x => x.Id == result); ;
 
        ShopCartVM.OrderHeader.Name = ShopCartVM.OrderHeader.AppUser.Name;
        ShopCartVM.OrderHeader.PhoneNumber=ShopCartVM.OrderHeader.AppUser.PhoneNumber;
            ShopCartVM.OrderHeader.City = ShopCartVM.OrderHeader.AppUser.city;
            ShopCartVM.OrderHeader.Adress = ShopCartVM.OrderHeader.AppUser.Adress;
        ShopCartVM.OrderHeader.Email = ShopCartVM.OrderHeader.AppUser.Email;
        ShopCartVM.OrderHeader.PostalCode = ShopCartVM.OrderHeader.AppUser.PostalCode;

            return View(ShopCartVM);
        }
        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            var user = (ClaimsIdentity)User.Identity;
            string result = user.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShopCartVM.ShoppingCarts = _unitOfWork._ShoppingCartRepository.GetAll("ShoppingCarts", $"Where UserId = '{result}'", IncludePr: "Product");
            if(ShopCartVM.ShoppingCarts.Count() == 0)
            {
                TempData["error"] = "No Products to be Ordered !!";
                return View(ShopCartVM);
            }
            //we should recalculate the totale because in the index action we are just storing it in memory not in db
            decimal totalPrice = 0;
            foreach (var cart in ShopCartVM.ShoppingCarts)
            {
                totalPrice += cart.Price * cart.Quantity;
            }
            ShopCartVM.OrderHeader.OrderTotal = totalPrice;

            ShopCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShopCartVM.OrderHeader.OrderStatus = SD.StatusAproved;
            ShopCartVM.OrderHeader.PaymentDate = DateOnly.FromDateTime(DateTime.Now);
            ShopCartVM.OrderHeader.PaymentStatus = SD.PaymentAproved;
            ShopCartVM.OrderHeader.UserId = result;

            _unitOfWork.AppDbContext().OrderHeaders.Add(ShopCartVM.OrderHeader);
            _unitOfWork.AppDbContext().SaveChanges();

            OrderDetail OrdDetail= new();
            foreach(var item in ShopCartVM.ShoppingCarts)
            {
                OrdDetail.OrderId = ShopCartVM.OrderHeader.Id;
                OrdDetail.ProductId = item.ProductId;
                OrdDetail.Quantity = item.Quantity;
                OrdDetail.Price = item.Price;
                _unitOfWork._OrderDetailRepository.Add(OrdDetail);
            }
            return RedirectToAction(nameof(ConfirmationOrder) , new {id = ShopCartVM.OrderHeader.Id});
        }
        public IActionResult ConfirmationOrder(int id)
        {
            var user = (ClaimsIdentity)User.Identity;
            string result = user.FindFirst(ClaimTypes.NameIdentifier).Value;

            OrderHeader order = _unitOfWork._OrderHeaderRepository.GetOne("OrderHeaders" , $"Where Id = {id}");

            IEnumerable<ShoppingCart>? shopCarts = _unitOfWork._ShoppingCartRepository.GetAll("ShoppingCarts", $"Where UserId = '{result}'");

            _unitOfWork.AppDbContext().ShoppingCarts.RemoveRange(shopCarts);
            _unitOfWork.AppDbContext().SaveChanges();
            return View(order);
        }
        public IActionResult Plus(int id)
        {
            var cart = _unitOfWork._ShoppingCartRepository.GetOne("ShoppingCarts","Where Id = "+ id);
            cart.Quantity += 1;
            _unitOfWork._ShoppingCartRepository.update(cart);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int id)
        {
            var cart = _unitOfWork._ShoppingCartRepository.GetOne("ShoppingCarts", "Where Id = " + id);
            if (cart.Quantity <= 1)
            {
                _unitOfWork._ShoppingCartRepository.Remove("ShoppingCarts", cart.Id);
                /*HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(x => x.userId == cart.userId).Count() - 1);*///in here -1 because when we remove we are not saving the changes so the changes is happening only in memory
            }
            else
            {
                cart.Quantity -= 1;
                _unitOfWork._ShoppingCartRepository.update(cart);
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int id)
        {
            //in here we are not tracking the cart because we already set it to false in the repository but when we call the remove that mean ef is tracking the cart
            var cart = _unitOfWork._ShoppingCartRepository.GetOne("ShoppingCarts", "Where Id = " + id);
            _unitOfWork._ShoppingCartRepository.Remove("ShoppingCarts" , cart.Id);
     
            //if we put this statement between getone and remove , an error will occured because we are tracking the same cart twice inside setint 32 first then when we call the remove
            /*HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(x => x.userId == cart.userId).Count());*/// we don't have to set -1 like Minus action because we already call the save changes

            return RedirectToAction(nameof(Index));
        }
    }
}
