using StoreManage.Entities;
using StoreManage.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Services.SellFactors.Contracts
{
    public interface SellFactorRepository : Repository
    {
        void Add(SellFactor sellfactor);
        SellFactor GetBySellFactorNumber(int sellFactorNumber);
        void Remove(SellFactor sellfactor);
        IList<GetSellFactorDto> GetAll();
        GetSellFactorDto GetSellFactor(int sellFactorNumber);
    }
}
