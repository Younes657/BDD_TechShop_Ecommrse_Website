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
    public class ProductRepositrry : Repository<Product>, IProductRepo
    {
        private readonly AppDbContext _db;
        public ProductRepositrry(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public void Add(Product product)
        {
            _ = _db.Database.ExecuteSql($"Insert into Products(ProductName ,Quantity ,ProDescription ,Price , ProImage, CategoryId , CompanyName) values({product.ProductName},{product.Quantity}, {product.Description}, {product.Price}, {product.ImageUrl},{product.CategoryId},{product.CompanyName})");
        }

        public void update(Product product)
        {
            _ = _db.Database.ExecuteSql($"Update Products Set ProductName= {product.ProductName} ,Quantity = {product.Quantity},ProDescription = {product.Description},Price = {product.Price} , ProImage = {product.ImageUrl}, CategoryId = {product.CategoryId},CompanyName= {product.CompanyName} where Id = {product.Id}");
        }
    }
}
