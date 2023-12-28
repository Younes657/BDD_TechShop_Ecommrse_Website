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
    public class OrderHeaderConfig : IEntityTypeConfiguration<OrderHeader>
    {
        public void Configure(EntityTypeBuilder<OrderHeader> builder)
        {
            builder.ToTable("OrderHeaders");
            builder.HasOne(x => x.Shipper).WithMany(x => x.OrderHeaders).HasForeignKey(x => x.ShipperId)
                .IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
