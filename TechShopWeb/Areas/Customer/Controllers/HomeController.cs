using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ShopTech.Models;
using ShopTech.DataAccess.Repository.IRepository;
using ShopTech.DataAccess.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ShopTech.Utility;

namespace TechShopWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;
        public HomeController(ILogger<HomeController> logger , IUnitOfWork unitofwork)
        {
            _logger = logger;
            unitOfWork = unitofwork;
        }

        public IActionResult Index()
        {
            var Products = unitOfWork._ProductRepository.GetAll("Products", IncludePr: "Category");
            return View(Products);
        }
        public IActionResult Detail(int? prdId)
        {
            if (prdId == null || prdId == 0)
            {
                return NotFound();
            }
            Product? prd = unitOfWork._ProductRepository.GetOne("Products","where Id = "+prdId,IncludePr:"Category");
            if (prd == null)
            {
                return NotFound();
            }
            //in here the ? is for checking the null objects if user is null it will return null directly it will not pass to the FindFirst methode;
            var user = (ClaimsIdentity)User.Identity;
            string? result = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ShoppingCart? Cart;
            if (result != null)
            {
                Cart = unitOfWork._ShoppingCartRepository.GetOne("ShoppingCarts", "Where ProductId = " + prdId + $" AND UserId = '{result}'");
                if(Cart == null)
                {
                    Cart = new ShoppingCart()
                    {
                        Product = prd,
                        Quantity = 1,
                        ProductId = prd.Id,
                        Price = prd.Price
                    };
                }
                else
                {
                    Cart.Product = prd;
                    return View(Cart);
                }
            }
            else
            {
                Cart = new ShoppingCart()
                {
                    Product = prd,
                    Quantity = 1,
                    ProductId = prd.Id,
                    Price = prd.Price
                };
            }
            return View(Cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Detail(ShoppingCart Cart)
        {
            var user = (ClaimsIdentity)User.Identity;
            var result = user.FindFirst(ClaimTypes.NameIdentifier).Value;

            Cart.UserId = result;
            ShoppingCart CartDb = unitOfWork._ShoppingCartRepository.GetOne("ShoppingCarts", "Where ProductId = " + Cart.ProductId + $" AND UserId = '{result}'",IncludePr:"Product");
            string msg;
           
            if (CartDb != null)
            {//cart already exist
                if (Cart.Quantity > CartDb.Product.Quantity)
                {
                    Cart.Quantity = CartDb.Product.Quantity;
                }

                CartDb.Quantity = Cart.Quantity;
                unitOfWork._ShoppingCartRepository.update(CartDb);
                msg = "Cart Updated successfully";
            }
            else
            {
                unitOfWork._ShoppingCartRepository.Add(Cart);
                msg = "Cart Added  successfully";

                HttpContext.Session.SetInt32(SD.SessionCart, unitOfWork._ShoppingCartRepository.GetAll("ShoppingCarts", $"where UserId = '{result}'").Count());
            }
            TempData["success"] = msg;
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
