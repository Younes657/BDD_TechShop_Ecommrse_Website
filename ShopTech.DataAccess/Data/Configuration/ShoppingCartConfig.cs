using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTech.DataAccess.Data.Configuration
{
    public class ShoppingCartConfig : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.ToTable("ShoppingCarts");
            builder.HasOne(x => x.Product).WithMany(x => x.shoppingCarts).HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.AppUser).WithMany(x => x.ShoppingCarts).HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
