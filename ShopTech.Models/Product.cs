using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ShopTech.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        //[Remote(action: "VerifyEmail", controller: "Users")] then we should implement the action and we can return a json and using ajax and jquery we can add that to the jquery validation
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string? CompanyName { get; set; }
        [ValidateNever]
        public Category Category { get; set; } = null!;
        [ValidateNever]
        public ICollection<ShoppingCart>? shoppingCarts { get; set; } = new List<ShoppingCart>();
        [ValidateNever]
        public ICollection<OrderDetail>? OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
