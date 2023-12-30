
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopTech.DataAccess.Data;
using ShopTech.Models;
using ShopTech.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTech.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly AppDbContext _db;
        public DbInitializer(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> userRole,
            AppDbContext db) 
        { 
            _UserManager = userManager;
            _RoleManager = userRole;
            _db = db;
        }  
        public void Initialize()
        {
            //we should create migration if thery are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex) { }
            // we create the roles
            if (!_RoleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _RoleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _RoleManager.CreateAsync(new IdentityRole(SD.Role_Cust)).GetAwaiter().GetResult();

                //then we create the first user admin
                _UserManager.CreateAsync(new ApplicationUser()
                {
                    UserName = "Younes",
                    Email = "younesbc2123@gmail.com",
                    Name = "Younes Bouzenacha",
                    PhoneNumber = "0793021107",
                    Adress = "New York city, 199 el elma",
                    city = "Setif",
                    PostalCode = "123-123-21"
                }, "123Younes@").GetAwaiter().GetResult();

                //now let's get the user 
                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(x => x.Email == "younesbc2123@gmail.com");
                _UserManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }
            return;
        }
    }
}
