using StoreManage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Test.Tools.Commodities
{
    public static class CommodityFactory
    {
        public static Commodity CreateCommodity(int categoryId)
        {
            return new Commodity
            {
                Name = "ماست رامک",
                Price = "150000",
                Inventory = 10,
                MaxInventory = "15",
                MinInventory = "5",
                CategoryId = categoryId,
            };
        }
    }
}
