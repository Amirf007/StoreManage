using StoreManage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Test.Tools.Commodities
{
    public static class CommoditiesFactory
    {
        public static List<Commodity> GenerateCommodities(int categoryId)
        {
            return new List<Commodity>
            {
                new Commodity
                {
                Code = 1,
                Name = "شیر رامک",
                Price = "150000",
                Inventory = 10,
                MaxInventory = "15",
                MinInventory = "5",
                CategoryId = categoryId,
                },
                new Commodity
                {
                Code= 2,
                Name = "دوغ رامک",
                Price = "120000",
                Inventory = 15,
                MaxInventory = "20",
                MinInventory = "7",
                CategoryId = categoryId,
                },
            };
        }
    }
}
