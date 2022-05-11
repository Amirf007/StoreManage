using StoreManage.Entities;
using StoreManage.Services.Commodities.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Persistence.EF.Commodities
{
    public class EFCommodityRepository : CommodityRepository
    {
        private EFDataContext _dataContext;

        public EFCommodityRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Commodity commodity)
        {
            _dataContext.Commodities.Add(commodity);
        }

        public IList<GetCommodityDto> GetAll()
        {
            return _dataContext.Commodities
                  .Select(_ => new GetCommodityDto
                  {
                     
                      Name = _.Name,
                      Price = _.Price,
                      Inventory = _.Inventory,
                      MaxInventory = _.MaxInventory,
                      MinInventory = _.MinInventory,
                      CategoryId = _.CategoryId,

                  }).ToList();
        }

        public Commodity GetbyCode(int code)
        {
            return _dataContext.Commodities.Find(code);
        }

        public GetCommodityDto GetCommodity(int code)
        {
            return _dataContext.Commodities
                .Where(_ => _.Code == code)
            .Select(_ => new GetCommodityDto
            {
                Name = _.Name,
                Price = _.Price,
                Inventory = _.Inventory,
                MaxInventory = _.MaxInventory,
                MinInventory = _.MinInventory,
                CategoryId = _.CategoryId,

            }).SingleOrDefault();
        }

        public bool IsExistCodeCommodity(int code)
        {
            return _dataContext.Commodities
               .Any(_ => _.Code == code);
        }

        public bool IsExistName(string name,int CategoryId, int code)
        {
            return _dataContext.Commodities
               .Any(_ => _.Name == name 
               && _.CategoryId == CategoryId && _.Code != code);
        }

        public bool IsExistNameCommodity(string name,int CategoryId)
        {
            return _dataContext.Commodities
                .Any(_ => _.Name==name && _.CategoryId == CategoryId);
        }

        public void Remove(Commodity commodity)
        {
            _dataContext.Commodities.Remove(commodity);
        }
    }
}
