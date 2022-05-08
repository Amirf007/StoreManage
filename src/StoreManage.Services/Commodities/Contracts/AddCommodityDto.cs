using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Services.Commodities.Contracts
{
    public class AddCommodityDto
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public int Inventory { get; set; }
        public string MaxInventory { get; set; }
        public string MinInventory { get; set; }
        public int CategoryId { get; set; }
    }
}
