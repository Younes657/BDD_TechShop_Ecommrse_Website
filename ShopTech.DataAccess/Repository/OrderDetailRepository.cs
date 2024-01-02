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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailsRepo
    {
        private readonly AppDbContext _db;
        public OrderDetailRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public void Add(OrderDetail ordDetail)
        {
            _ = _db.Database.ExecuteSql($"Insert into OrderDetails(OrderId ,ProductId ,Price ,Quantity) values({ordDetail.OrderId},{ordDetail.ProductId}, {ordDetail.Price}, {ordDetail.Quantity})");
        }

    }
}
