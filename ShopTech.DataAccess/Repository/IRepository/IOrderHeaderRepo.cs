using ShopTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTech.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepo : IRepository<OrderHeader>
    {
        void Add(OrderHeader ordHeader);
        void Update(OrderHeader ordHeader);
        void UpdateStatus(int id , string OrderStatus , string? PaymentStatus = null);
    }
}
