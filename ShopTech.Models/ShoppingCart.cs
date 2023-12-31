using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ShopTech.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; } = null!;
        [Required]
        public string UserId { get; set; }
        [ValidateNever]
        public ApplicationUser AppUser { get; set; } = null!;
        public decimal Price { get; set; }
        [Range(1, 1000, ErrorMessage = "Invalid Number for this field")]
        public int Quantity { get; set; }
    }
}
