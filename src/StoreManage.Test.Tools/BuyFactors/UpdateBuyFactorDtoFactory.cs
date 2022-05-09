using StoreManage.Services.BuyFactors.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Test.Tools.BuyFactors
{
    public static class UpdateBuyFactorDtoFactory
    {
        public static UpdateBuyFactorDto GenerateUpdateBuyFactorDto(int commoditycode)
        {
            return new UpdateBuyFactorDto
            {
                CommodityCode = commoditycode,
                Date = DateTime.Now.Date,
                Count = 3,
                BuyPrice = "130000",
                SellerName = "amir asady"
            };
        }
    }
}
