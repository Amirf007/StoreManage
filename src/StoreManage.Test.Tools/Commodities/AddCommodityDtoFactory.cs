using StoreManage.Services.Commodities.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Test.Tools.Commodities
{
    public static class AddCommodityDtoFactory
    {
        public static AddCommodityDto GenerateAddCommodityDto(int categoryId)
        {
            return new AddCommodityDto
            {
                Code = 1,
                Name = "شیر رامک",
                Price = "150000",
                Inventory = 10,
                MaxInventory = "15",
                MinInventory = "5",
                CategoryId = categoryId,
            };
        }
    }
}
