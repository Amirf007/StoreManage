using StoreManage.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Services.Commodities.Contracts
{
    public interface CommodityService : Service
    {
        void Add(AddCommodityDto dto);
        void Update(int code, UpdateCommodityDto dto);
        void Delete(int code);
        IList<GetCommodityDto> GetAll();
        GetCommodityDto GetCommodity(int code);
    }
}
