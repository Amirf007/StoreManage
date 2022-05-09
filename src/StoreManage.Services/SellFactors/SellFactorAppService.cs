using StoreManage.Entities;
using StoreManage.Infrastructure.Application;
using StoreManage.Services.Commodities.Contracts;
using StoreManage.Services.SellFactors.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Services.SellFactors
{
    public class SellFactorAppService : SellFactorService
    {
        private SellFactorRepository _repository;
        private UnitOfWork _unitOfWork;
        private CommodityRepository _commodityRepository;

        public SellFactorAppService(SellFactorRepository sellfactorrepository, UnitOfWork unitOfWork, CommodityRepository commodityRepository)
        {
            _repository = sellfactorrepository;
            _unitOfWork = unitOfWork;
            _commodityRepository = commodityRepository;
        }

        public void Add(AddSellFactorDto dto)
        {
            SellFactor sellfactor = new SellFactor
            {
                CommodityCode = dto.CommodityCode,
                Date = dto.Date,
                Count = dto.Count,
                BasePrice = dto.BasePrice,
                TotalPrice = dto.TotalPrice,
                BuyerName = dto.BuyerName
            };

            _repository.Add(sellfactor);

            Commodity commodity = _commodityRepository.GetbyId(sellfactor.CommodityCode);
            commodity.Inventory -= sellfactor.Count;

            _unitOfWork.Commit();
        }
    }
}
