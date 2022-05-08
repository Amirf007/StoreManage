using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreManage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Persistence.EF.Commodities
{
    public class CommodityEntityMap : IEntityTypeConfiguration<Commodity>
    {
        public void Configure(EntityTypeBuilder<Commodity> _)
        {
            _.ToTable("Commodities");

            _.HasKey(_ => _.Code);
            _.Property(_ => _.Code)
                .IsRequired();

            _.Property(_ => _.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);

            _.Property(_ => _.Price)
                .IsRequired()
                .HasMaxLength(50);

            _.Property(_ => _.Inventory)
                .IsRequired();

            _.Property(_ => _.MaxInventory)
                .IsRequired()
                .HasMaxLength(50);

            _.Property(_ => _.MinInventory)
                .IsRequired()
                .HasMaxLength(50);

            _.Property(_ => _.CategoryId)
                .IsRequired();

            _.HasOne(_ => _.Category)
                .WithMany(_ => _.Commodities)
                .HasForeignKey(_ => _.CategoryId);

            _.HasMany(_ => _.BuyFactors)
                .WithOne(_ => _.Commodity)
                .HasForeignKey(_ => _.CommodityCode);

            _.HasMany(_ => _.SellFactors)
                .WithOne(_ => _.Commodity)
                .HasForeignKey(_ => _.CommodityCode);
        }
    }
}
