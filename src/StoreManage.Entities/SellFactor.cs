using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Entities
{
    public class SellFactor
    {
        public int SellFactorNumber { get; set; }
        public DateTime Date { get; set; }
        public string Count { get; set; }
        public string BasePrice { get; set; }
        public string TotalPrice { get; set; }
        public string BuyerName { get; set; }
        public Commodity Commodity { get; set; }
        public int CommodityCode { get; set; }
    }
}
