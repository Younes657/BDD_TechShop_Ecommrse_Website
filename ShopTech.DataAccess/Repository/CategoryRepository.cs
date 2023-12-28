using Microsoft.EntityFrameworkCore;
using ShopTech.DataAccess.Data;
using ShopTech.DataAccess.Repository.IRepository;
using ShopTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTech.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepo
    {
        private readonly AppDbContext _db;
        public CategoryRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public void Add(Category category)
        {
            _ = _db.Database.ExecuteSql($"Insert into Categories(CategoryName , CatDescription) values({category.CategoryName},{category.Description})");
        }

        public void Update(Category category)
        {
            _ = _db.Database.ExecuteSql($"Update Categories Set CategoryName = {category.CategoryName}, CatDescription = {category.Description} where Id =  {category.Id}");
                
        }
    }
}
