using ShopTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTech.DataAccess.Repository.IRepository
{
    public interface ICategoryRepo : IRepository<Category>
    {
        void Add(Category category);
        void Update(Category category);
    }
}
