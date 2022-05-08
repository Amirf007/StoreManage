using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreManage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Persistence.EF.BuyFactors
{
    public class BuyFactorEntityMap : IEntityTypeConfiguration<BuyFactor>
    {
        public void Configure(EntityTypeBuilder<BuyFactor> _)
        {
            _.ToTable("BuyFactors");

            _.HasKey(_ => _.BuyFactorNumber);
            _.Property(_ => _.BuyFactorNumber)
                .ValueGeneratedOnAdd();

            _.Property(_ => _.Date)
                .IsRequired();

            _.Property(_ => _.BuyPrice)
                .IsRequired()
                .HasMaxLength(50);

            _.Property(_ => _.Count)
                .IsRequired();

            _.Property(_ => _.SellerName)
                .IsRequired()
                .HasMaxLength(50);

            _.Property(_ => _.CommodityCode)
                .IsRequired();
        }
    }
}
