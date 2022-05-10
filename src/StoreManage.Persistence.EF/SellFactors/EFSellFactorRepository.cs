using StoreManage.Entities;
using StoreManage.Services.SellFactors.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Persistence.EF.SellFactors
{
    public class EFSellFactorRepository : SellFactorRepository
    {
        private EFDataContext _dataContext;

        public EFSellFactorRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(SellFactor sellfactor)
        {
            _dataContext.SellFactors.Add(sellfactor);
        }

        public SellFactor GetBySellFactorNumber(int sellFactorNumber)
        {
            return _dataContext.SellFactors.Find(sellFactorNumber);
        }

        public void Remove(SellFactor sellfactor)
        {
            _dataContext.SellFactors.Remove(sellfactor);
        }
    }
}
