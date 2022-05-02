using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Migrations
{
    [Migration(202205020930)]
    public class _202205020930_initalTables : Migration
    {
        public override void Up()
        {
            CreateCategoriesTable();
            CreateCommoditiesTable();
            CreateBuyFactorsTable();
            CreateSellFactorsTable();

            CreateRelationships();
        }

        private void CreateRelationships()
        {
            Create.ForeignKey("FK_Commodities_Categories")
                    .FromTable("Commodities").ForeignColumns("CategoryId")
                    .ToTable("Categories").PrimaryColumn("Id")
                    .OnDeleteOrUpdate(System.Data.Rule.None);

            Create.ForeignKey("FK_SellFactors_Commodities")
                    .FromTable("SellFactors").ForeignColumns("CommodityCode")
                    .ToTable("Commodities").PrimaryColumn("Code")
                    .OnDeleteOrUpdate(System.Data.Rule.None);

            Create.ForeignKey("FK_BuyFactors_Commodities")
                    .FromTable("BuyFactors").ForeignColumns("CommodityCode")
                    .ToTable("Commodities").PrimaryColumn("Code")
                    .OnDeleteOrUpdate(System.Data.Rule.None);
        }

        private void CreateSellFactorsTable()
        {
            Create.Table("SellFactors")
                 .WithColumn("SellFactorNumber").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("Count").AsString(50).NotNullable()
                .WithColumn("BasePrice").AsString(50).NotNullable()
                .WithColumn("TotalPrice").AsString(50).NotNullable()
                .WithColumn("BuyerName").AsString(50).NotNullable()
                .WithColumn("CommodityCode").AsInt32().NotNullable();
        }

        private void CreateBuyFactorsTable()
        {
            Create.Table("BuyFactors")
                             .WithColumn("BuyFactorNumber").AsInt32().PrimaryKey().NotNullable().Identity()
                            .WithColumn("Date").AsDateTime().NotNullable()
                            .WithColumn("BuyPrice").AsString(50).NotNullable()
                            .WithColumn("Count").AsString(50).NotNullable()
                            .WithColumn("SellerName").AsString(50).NotNullable()
                            .WithColumn("CommodityCode").AsInt32().NotNullable();
        }

        private void CreateCommoditiesTable()
        {
            Create.Table("Commodities")
                 .WithColumn("Code").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("Name").AsString(50).Unique().NotNullable()
                .WithColumn("Price").AsString(50).NotNullable()
                .WithColumn("Inventory").AsInt32().NotNullable()
                .WithColumn("MaxInventory").AsString(50).NotNullable()
                .WithColumn("MinInventory").AsString(50).NotNullable()
                .WithColumn("CategoryId").AsInt32().NotNullable();
        }

        private void CreateCategoriesTable()
        {
            Create.Table("Categories")
                             .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
                            .WithColumn("Title").AsString(50).Unique().NotNullable();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Commodities_Categories");
            Delete.ForeignKey("FK_SellFactors_Commodities");
            Delete.ForeignKey("FK_BuyFactors_Commodities");
            Delete.Table("Categories");
            Delete.Table("Commodities");
            Delete.Table("SellFactors");
            Delete.Table("BuyFactors");
        }
    }
}
