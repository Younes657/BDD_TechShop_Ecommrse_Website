using Microsoft.EntityFrameworkCore;
using ShopTech.DataAccess.Data;
using ShopTech.DataAccess.Repository.IRepository;
using ShopTech.Models;
using ShopTech.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTech.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepo
    {
        private readonly AppDbContext _db;
        public OrderHeaderRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public void Add(OrderHeader ordHeader)
        {
            _ = _db.Database.ExecuteSql($"Insert into OrderHeaders(UserId ,OrderDate ,OrderShippingDate , OrderTotal ,OrderStatus ,PaymentStatus ,PaymentDate ,TrackingNumber, Name ,Adress, PhoneNumber,PostalCode ,Email ,City) values({ordHeader.UserId},{ordHeader.OrderDate},{ordHeader.OrderShippingDate},{ordHeader.OrderTotal},{ordHeader.OrderStatus},{ordHeader.PaymentStatus},{ordHeader.PaymentDate},{ordHeader.TrackingNumber},{ordHeader.Name},{ordHeader.Adress},{ordHeader.PhoneNumber},{ordHeader.PostalCode},{ordHeader.Email},{ordHeader.City})");
        }

        public void Update(OrderHeader ordHeader)
        {
            _ = _db.Database.ExecuteSql($"Update OrderHeaders Set UserId = {ordHeader.UserId} ,OrderDate={ordHeader.OrderDate} ,OrderShippingDate={ordHeader.OrderShippingDate} , OrderTotal ={ordHeader.OrderTotal},OrderStatus={ordHeader.OrderStatus} ,PaymentStatus={ordHeader.PaymentStatus} ,PaymentDate={ordHeader.PaymentDate} ,ShipperId= {ordHeader.ShipperId} ,TrackingNumber={ordHeader.TrackingNumber}, Name={ordHeader.Name} ,Adress={ordHeader.Adress}, PhoneNumber={ordHeader.PhoneNumber},PostalCode={ordHeader.PostalCode} ,Email={ordHeader.Email} ,City={ordHeader.City}");
        }
        public void UpdateStatus(int id , string OrderStatus , string? PaymentStatus = null)
        {
            if (string.IsNullOrEmpty(PaymentStatus))
            {
                _ = _db.Database.ExecuteSql($"Update OrderHeaders Set OrderStatus = {OrderStatus} Where Id = {id}");
            }
            else
            {
                _ = _db.Database.ExecuteSql($"Update OrderHeaders Set OrderStatus = {OrderStatus} ,PaymentStatus = {PaymentStatus} Where Id = {id}");
            }
        }
    }
}
