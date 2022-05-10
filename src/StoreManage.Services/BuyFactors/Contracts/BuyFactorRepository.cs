using StoreManage.Entities;
using StoreManage.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Services.BuyFactors.Contracts
{
    public interface BuyFactorRepository : Repository
    {
        void Add(BuyFactor buyfactor);
        BuyFactor GetbyFactorNumber(int buyFactorNumber);
        void Remove(BuyFactor buyfactor);
        IList<GetBuyFactorDto> GetAll();
        GetBuyFactorDto GetBuyFactor(int buyFactorNumber);
    }
}
