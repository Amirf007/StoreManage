using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreManage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Persistence.EF.SellFactors
{
    public class SellFactorEntityMap : IEntityTypeConfiguration<SellFactor>
    {
        public void Configure(EntityTypeBuilder<SellFactor> _)
        {
            _.ToTable("SellFactors");

            _.HasKey(_ => _.SellFactorNumber);
            _.Property(_ => _.SellFactorNumber)
                .ValueGeneratedOnAdd();

            _.Property(_ => _.Date)
                .IsRequired();

            _.Property(_ => _.Count)
                .IsRequired()
                .HasMaxLength(50);

            _.Property(_ => _.BasePrice)
                .IsRequired()
                .HasMaxLength(50);

            _.Property(_ => _.TotalPrice)
                .IsRequired()
                .HasMaxLength(50);

            _.Property(_ => _.BuyerName)
                .IsRequired()
                .HasMaxLength(50);

            _.Property(_ => _.CommodityCode)
                .IsRequired();
        }
    }
}
