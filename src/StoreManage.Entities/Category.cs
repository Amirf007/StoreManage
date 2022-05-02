using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Commodity> Commodities { get; set; }
    }
}
