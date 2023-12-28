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
    public class OrderDetailsConfig : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(x => new { x.OrderId, x.ProductId });
            builder.ToTable("OrderDetails");
            builder.HasOne(x => x.OrderHeader).WithMany(x => x.OrderDetails).HasForeignKey(x => x.OrderId)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Product).WithMany(x => x.OrderDetails).HasForeignKey(x => x.ProductId)
               .IsRequired().OnDelete(DeleteBehavior.NoAction);
        }
    }
}
