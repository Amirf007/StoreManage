using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Entities
{
    public class BuyFactor
    {
        public int BuyFactorNumber { get; set; }
        public DateTime Date { get; set; }
        public string BuyPrice { get; set; }
        public int Count { get; set; }
        public string SellerName { get; set; }
        public Commodity Commodity { get; set; }
        public int CommodityCode { get; set; }
    }
}
