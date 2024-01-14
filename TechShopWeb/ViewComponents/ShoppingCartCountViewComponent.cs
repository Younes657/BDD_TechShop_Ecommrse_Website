
using Microsoft.AspNetCore.Mvc;
using ShopTech.DataAccess.Repository.IRepository;
using ShopTech.Utility;
using System.Security.Claims;

namespace BulkyWebV2.ViewComponents
{
    public class ShoppingCartCountViewComponent : ViewComponent
    {
        public IUnitOfWork _UnitOfWork { get; set; }
        public ShoppingCartCountViewComponent(IUnitOfWork unitOf)
        {
            _UnitOfWork = unitOf;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = (ClaimsIdentity)User.Identity;
            var result = user.FindFirst(ClaimTypes.NameIdentifier);

            if (result != null)
            {
                if (HttpContext.Session.GetInt32(SD.SessionCart) == null)
                {
                    HttpContext.Session.SetInt32(SD.SessionCart, _UnitOfWork._ShoppingCartRepository.GetAll("ShoppingCarts" , $"where UserId = '{result.Value}'").Count());
                }
                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }
            else
            {
                //means there is no user and we can remove the code in the logout action
                HttpContext.Session.Clear();
                return View(0);
            }

        }

    }
}
