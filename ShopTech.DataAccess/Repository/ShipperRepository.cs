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
    public class ShipperRepository : Repository<Shipper>, IShipperRepo
    {
        private readonly AppDbContext _db;
        public ShipperRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public void Add(Shipper shipper)
        {
            _ = _db.Database.ExecuteSql($"Insert into Shippers(CompanyName ,Phone ,Email) values ({shipper.CompanyName},{shipper.Phone},{shipper.Email})");
        }

        public void update(Shipper shipper)
        {
            _ = _db.Database.ExecuteSql($"Update Shippers Set CompanyName= {shipper.CompanyName} ,Phone = {shipper.Phone},Email = {shipper.Email} Where Id = {shipper.Id}");
        }
    }
}
