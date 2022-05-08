using StoreManage.Entities;
using StoreManage.Services.BuyFactors.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Persistence.EF.BuyFactors
{
    public class EFBuyFactorRepository : BuyFactorRepository
    {
        private EFDataContext _dataContext;

        public EFBuyFactorRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(BuyFactor buyfactor)
        {
            _dataContext.Add(buyfactor);
        }
    }
}
