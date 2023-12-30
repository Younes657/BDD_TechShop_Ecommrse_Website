using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTech.Models.VModels
{
    [NotMapped]
    public class Register
    {
        [Required]
        public string Name { get; set; }
        public string? Adress { get; set; }
        public string? City { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PostalCode { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirmation Password did not match !!")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string? Role { get; set; }
    }
}
