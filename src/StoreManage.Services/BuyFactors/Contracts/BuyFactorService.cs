using StoreManage.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Services.BuyFactors.Contracts
{
    public interface BuyFactorService : Service
    {
        void Add(AddBuyFactorDto dto);
    }
}
