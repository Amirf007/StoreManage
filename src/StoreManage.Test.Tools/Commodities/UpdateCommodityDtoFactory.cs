using StoreManage.Services.Commodities.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Test.Tools.Commodities
{
    public static class UpdateCommodityDtoFactory
    {
        public static UpdateCommodityDto GenerateUpdateCommodityDto(int categoryId)
        {
            return new UpdateCommodityDto
            {
                Name = "شیر رامک",
                Price = "170000",
                Inventory = 10,
                MaxInventory = "15",
                MinInventory = "5",
                CategoryId = categoryId,
            };
        }
    }
}
