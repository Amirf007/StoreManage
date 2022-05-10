using StoreManage.Services.SellFactors.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Test.Tools.SellFactors
{
    public static class UpdateSellFactorDtoFactory
    {
        public static UpdateSellFactorDto GenerateUpdateSellFactorDto(int commoditycode)
        {
            return new UpdateSellFactorDto
            {
                CommodityCode = commoditycode,
                Date = DateTime.Now.Date,
                Count = 2,
                BasePrice = "150000",
                TotalPrice = "300000",
                BuyerName = "amir asady"
            };
        }
    }
}
