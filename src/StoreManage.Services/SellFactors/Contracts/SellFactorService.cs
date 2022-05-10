using StoreManage.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Services.SellFactors.Contracts
{
    public interface SellFactorService : Service
    {
        void Add(AddSellFactorDto dto);
        void Update(int sellFactorNumber, UpdateSellFactorDto dto);
        void Delete(int sellFactorNumber);
        IList<GetSellFactorDto> GetAll();
        GetSellFactorDto GetSellFactor(int sellFactorNumber);
    }
}
