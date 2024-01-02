using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopTech.Models
{
    public class Shipper
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone {  get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public ICollection<OrderHeader>? OrderHeaders { get; set; } = new List<OrderHeader>();

    }
}
