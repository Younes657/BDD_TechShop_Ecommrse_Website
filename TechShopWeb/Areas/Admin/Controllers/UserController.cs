using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTech.DataAccess.Repository.IRepository;
using ShopTech.Utility;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TechShopWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    [Area("Admin")]
    public class UserController : Controller
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public UserController(IUnitOfWork unitofwork)
        {
            UnitOfWork = unitofwork;
        }
        public IActionResult Index()
        {
            var users = UnitOfWork.AppDbContext().ApplicationUsers.ToList();
            var UserRole = UnitOfWork.AppDbContext().UserRoles.ToList();
            var Roles = UnitOfWork.AppDbContext().Roles.ToList();
            foreach (var user in users)
            {
                var userrole = UserRole.FirstOrDefault(x => x.UserId == user.Id);
                var role = Roles.FirstOrDefault(x => x.Id == userrole.RoleId);
                user.Role = role.Name;
            }
            return View(users);
        }
        public IActionResult LockUnlock(string id)
        {
            var UserDb = UnitOfWork.AppDbContext().ApplicationUsers.FromSqlRaw($"Select * from AspNetUsers Where Id = '{id}'").FirstOrDefault();
            if(UserDb == null)
            {
                TempData["error"] = "Error When Locking/Unlocking !!";
                return RedirectToAction(nameof(Index));
            }
            if (UserDb.LockoutEnd != null && UserDb.LockoutEnd > DateTime.Now)
            {
                //user is currently locked and we need to unlock it
                UserDb.LockoutEnd = DateTime.Now;
                TempData["success"] = "User Unlocked successfuly";
            }
            else
            {
                UserDb.LockoutEnd = DateTime.Now.AddYears(100);
                TempData["success"] = "User locked successfuly";
            }
            UnitOfWork.AppDbContext().SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        #region Api Calls
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = UnitOfWork.AppDbContext().ApplicationUsers.ToList();
            return Json(data: users);
        }
        #endregion
    }
}
