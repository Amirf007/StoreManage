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

        public void Remove(BuyFactor buyfactor)
        {
            _dataContext.BuyFactors.Remove(buyfactor);
        }

        public IList<GetBuyFactorDto> GetAll()
        {
            return _dataContext.BuyFactors
                 .Select(_ => new GetBuyFactorDto
                 {
                     CommodityCode = _.CommodityCode,
                     Date = _.Date,
                     Count = _.Count,
                     BuyPrice = _.BuyPrice,
                     SellerName = _.SellerName,

                 }).ToList();
        }

        public GetBuyFactorDto GetBuyFactor(int buyFactorNumber)
        {
            return _dataContext.BuyFactors
                .Where(_ => _.BuyFactorNumber == buyFactorNumber)
            .Select(_ => new GetBuyFactorDto
            {
                CommodityCode = _.CommodityCode,
                Date = _.Date,
                Count = _.Count,
                BuyPrice = _.BuyPrice,
                SellerName = _.SellerName,

            }).SingleOrDefault();
        }

        public BuyFactor GetbyFactorNumber(int buyFactorNumber)
        {
            return _dataContext.BuyFactors.Find(buyFactorNumber);
        }
    }
}
