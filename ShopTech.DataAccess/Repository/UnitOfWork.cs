using ShopTech.DataAccess.Data;
using ShopTech.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTech.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepo _CategoryRepository { get; private set; }
        public IProductRepo _ProductRepository { get; private set; }
        public IShoppingCartRepo _ShoppingCartRepository { get; private set; }
        public IShipperRepo _ShipperRepository { get; private set; }
        public IOrderHeaderRepo _OrderHeaderRepository { get; private set; }
        public IOrderDetailsRepo _OrderDetailRepository { get; private set; }

        private readonly AppDbContext _db;
        public UnitOfWork( AppDbContext db)
        {
            _db = db;
            _CategoryRepository = new CategoryRepository(_db);
            _ProductRepository = new ProductRepositrry(_db);
            _ShoppingCartRepository = new ShoppingCartRepositrry(_db);
            _ShipperRepository = new ShipperRepository(_db);
            _OrderHeaderRepository = new OrderHeaderRepository(_db);
            _OrderDetailRepository = new OrderDetailRepository(_db);
        }

        public AppDbContext AppDbContext()
        {
            return _db;
        }
    }
}
