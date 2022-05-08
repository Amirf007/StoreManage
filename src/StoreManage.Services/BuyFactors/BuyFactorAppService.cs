using StoreManage.Infrastructure.Application;
using StoreManage.Services.BuyFactors.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Services.BuyFactors
{
    public class BuyFactorAppService : BuyFactorService
    {
        private BuyFactorRepository _buyfactorrepository;
        private UnitOfWork _unitOfWork;

        public BuyFactorAppService(BuyFactorRepository buyfactorrepository, UnitOfWork unitOfWork)
        {
            _buyfactorrepository = buyfactorrepository;
            _unitOfWork = unitOfWork;
        }
    }
}
