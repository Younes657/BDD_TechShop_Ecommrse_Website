using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ShopTech.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [ValidateNever]
        public ApplicationUser AppUser { get; set; } = null!;
        [Required]
        public DateOnly OrderDate { get; set; }
        public DateOnly OrderShippingDate { get; set; }
        public decimal OrderTotal { get; set; }
        public string OrderStatus { get; set;}
        public string PaymentStatus { get; set;}
        public DateOnly PaymentDate { get; set; }
        public int ShipperId { get; set; }
        [ValidateNever]
        public Shipper Shipper { get; set; }
        public int TrackingNumber { get; set; }
        //info about the customer

        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Email { get; set; }
        [ValidateNever]
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
