using ShopTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTech.DataAccess.Repository.IRepository
{
    public interface IProductRepo : IRepository<Product>
    {
        void Add(Product product);
        void update(Product product);
    }
}
