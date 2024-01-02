
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopTech.Models;
using ShopTech.Models.VModels;
using ShopTech.Utility;
using System.Security.Claims;

namespace TechShopWeb.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly SignInManager<IdentityUser> _SignInManager;
        public AuthenticationController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> userRole,
            SignInManager<IdentityUser> signUser)
        {
            _UserManager = userManager;
            _RoleManager = userRole;
            _SignInManager = signUser;
        }

        public IActionResult Register()
        {
            IEnumerable<SelectListItem> ListRoles = _RoleManager.Roles.Select(
                 r => new SelectListItem
                 {
                     Text = r.Name,
                     Value = r.Name
                 });
            RegisterVM rgvm = new()
            {
                Register = new Register(),
                RoleList = ListRoles,
            };
            return View(rgvm);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            IEnumerable<SelectListItem> ListRoles = _RoleManager.Roles.Select(
                 r => new SelectListItem
                 {
                     Text = r.Name,
                     Value = r.Name
                 });

            registerVM.RoleList = ListRoles;

            if (ModelState.IsValid)
            {
                //check if the user exist
                var userExist = await _UserManager.FindByEmailAsync(registerVM.Register.Email);
                if (userExist != null)
                {
                    ViewData["message"] = "User Already Exist try to Log In !!!!";
                    return View(registerVM);
                }

                ApplicationUser user = new ApplicationUser()
                {
                    Email = registerVM.Register.Email,
                    UserName = registerVM.Register.UserName,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Name = registerVM.Register.Name,
                    city = registerVM.Register.City,
                    PhoneNumber = registerVM.Register.PhoneNumber,
                    Adress = registerVM.Register.Adress,
                    PostalCode = registerVM.Register.PostalCode,
                };

                //Represents the result of an identity operation.
                IdentityResult result = await _UserManager.CreateAsync(user, registerVM.Register.Password);

                if (!result.Succeeded)
                {
                    ViewData["error"] = "User Failed to Create !!!";
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(registerVM);
                }
                List<Claim> claims;
                claims =
                [
                    new Claim(ClaimTypes.NameIdentifier, registerVM.Register.UserName),
                    new Claim(ClaimTypes.Role, registerVM.Register.Role ?? SD.Role_Cust)
                ];
                var res = await _UserManager.AddClaimsAsync(user, claims);
                //var authenticationProperties = new AuthenticationProperties()
                //{
                //	AllowRefresh = true,
                //	IsPersistent = loginVM.RememberMe
                //};
                await _UserManager.AddToRoleAsync(user, registerVM.Register.Role ?? SD.Role_Cust);
                if (User.IsInRole(SD.Role_Admin))
                {
                    TempData["success"] = "User Created Succefully";
                    return View(registerVM);
                }
                else
                {
                    await _SignInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "Home", new {area="Customer"});
                }
            }
            return View(registerVM);
        }
        public IActionResult Login()
        {
            if (_SignInManager.IsSignedIn(User))
            {
                return View("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _SignInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, loginVM.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home" , new {area="Customer"});
                }
            }
            ModelState.AddModelError("", "failed to login try again ");
            return View();
        }
        [HttpPost]
        public IActionResult LogOut()
        {
            _SignInManager.SignOutAsync();
            //HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
