using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShopTech.Models
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; } = null!;
        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
