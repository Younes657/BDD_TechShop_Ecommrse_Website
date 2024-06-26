﻿using ShopTech.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTech.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
         ICategoryRepo _CategoryRepository { get; }
         IProductRepo _ProductRepository { get; }
         IShoppingCartRepo _ShoppingCartRepository { get; }
         IShipperRepo _ShipperRepository { get; }
         IOrderHeaderRepo _OrderHeaderRepository { get; }
         IOrderDetailsRepo _OrderDetailRepository { get; }

        AppDbContext AppDbContext();
    }
}
