using StoreManage.Services.BuyFactors.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Test.Tools.BuyFactors
{
    public static class AddBuyFactorDtoFactory
    {
        public static AddBuyFactorDto GenerateAddBuyFactorDto(int commoditycode)
        {
            return new AddBuyFactorDto
            {
                CommodityCode = commoditycode,
                Date = DateTime.Now.Date,
                Count = 4,
                BuyPrice = "125000",
                SellerName = "amir asady"
            };
        }
    }
}
