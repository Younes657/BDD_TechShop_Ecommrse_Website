using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.VisualBasic;

namespace ShopTech.Models.VModels
{
    [NotMapped]
    public class RegisterVM
    {
        public Register Register {  get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? RoleList {  get; set; }
    }
}
