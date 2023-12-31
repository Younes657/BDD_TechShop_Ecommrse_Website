using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTech.Models.VModels
{
    public class ShopingCartVM
    {
        public IEnumerable<ShoppingCart>? ShoppingCarts { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
