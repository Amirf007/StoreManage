using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Services.SellFactors.Contracts
{
    public class AddSellFactorDto
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public string BasePrice { get; set; }
        public string TotalPrice { get; set; }
        public string BuyerName { get; set; }
        public int CommodityCode { get; set; }
    }
}
