namespace ShopTech.Models
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public OrderHeader OrderHeader { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public float Discount { get; set; }
    }
}
