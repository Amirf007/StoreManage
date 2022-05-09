using StoreManage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Test.Tools.BuyFactors
{
    public static class BuyFactorFactory
    {
        public static BuyFactor GenerateBuyFactor(int commoditycode)
        {
            return new BuyFactor
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
