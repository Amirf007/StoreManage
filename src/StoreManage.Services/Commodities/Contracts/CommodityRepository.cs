using StoreManage.Entities;
using StoreManage.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Services.Commodities.Contracts
{
    public interface CommodityRepository : Repository
    {
        void Add(Commodity commodity);
        bool IsExistNameCommodity(string name, int CategoryId);
        Commodity GetbyId(int code);
        bool IsExistName(string name,int CategoryId, int code);
    }
}
