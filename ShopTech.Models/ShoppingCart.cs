using System.ComponentModel.DataAnnotations;

namespace ShopTech.Models
{
    public class ShoppingCart
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string UserId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
