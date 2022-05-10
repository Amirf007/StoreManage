using StoreManage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Test.Tools.SellFactors
{
    public static class SellFactorFactory
    {
        public static SellFactor GenerateSellFactor(int commoditycode)
        {
            return new SellFactor
            {
                CommodityCode = commoditycode,
                Date = DateTime.Now.Date,
                Count = 3,
                BasePrice = "150000",
                TotalPrice = "450000",
                BuyerName = "amir asady"
            };
        }
    }
}
