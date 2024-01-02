using ShopTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTech.DataAccess.Repository.IRepository
{
    public interface IShipperRepo : IRepository<Shipper>
    {
        void Add(Shipper shipper);
        void update(Shipper shipper);
    }
}
