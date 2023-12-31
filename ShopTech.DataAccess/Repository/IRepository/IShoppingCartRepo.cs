using ShopTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTech.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepo : IRepository<ShoppingCart>
    {
        void Add(ShoppingCart ShopCart);
        void update(ShoppingCart ShopCart);
    }
}
