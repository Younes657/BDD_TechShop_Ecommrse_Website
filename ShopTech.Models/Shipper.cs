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
        public string Phone {  get; set; }
        public string Email { get; set; }
        public ICollection<OrderHeader>? OrderHeaders { get; set; } = new List<OrderHeader>();

    }
}
