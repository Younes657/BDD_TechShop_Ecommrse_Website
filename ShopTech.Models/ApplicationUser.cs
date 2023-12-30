using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTech.Models
{
    
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string? Adress { get; set; }
        public string? city { get; set; }
        public string? PostalCode { get; set; }
        [ValidateNever]
        public ICollection<OrderHeader> OrderHeaders { get; set; } = new List<OrderHeader>();
        [ValidateNever]
        public ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
    }
}
