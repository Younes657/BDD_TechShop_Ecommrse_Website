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
    public class ShoppingCartRepositrry : Repository<ShoppingCart>, IShoppingCartRepo
    {
        private readonly AppDbContext _db;
        public ShoppingCartRepositrry(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public void Add(ShoppingCart ShopCart)
        {
            _ = _db.Database.ExecuteSql($"Insert into ShoppingCarts(ProductId, UserId,Quantity ,Price) values({ShopCart.ProductId},{ShopCart.UserId}, {ShopCart.Quantity}, {ShopCart.Price})");
        }

        public void update(ShoppingCart ShopCart)
        {
            _ = _db.Database.ExecuteSql($"Update ShoppingCarts Set ProductId= {ShopCart.ProductId} ,Quantity = {ShopCart.Quantity},UserId = {ShopCart.UserId}, Price = {ShopCart.Price} where Id = {ShopCart.Id}");
        }
    }
}
