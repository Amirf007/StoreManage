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

        public SellFactorAppService(SellFactorRepository sellfactorrepository
            , UnitOfWork unitOfWork, CommodityRepository commodityRepository)
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

            Commodity commodity = _commodityRepository
                .GetbyCode(sellfactor.CommodityCode);
            commodity.Inventory -= sellfactor.Count;

            _unitOfWork.Commit();

            if (commodity.Inventory <= int.Parse(commodity.MinInventory))
            {
                throw new EqualOrLessInventoryThanMinimumCommodityInventoryException();
            }
        }

        public void Delete(int sellFactorNumber)
        {
            var sellfactor=_repository.GetBySellFactorNumber(sellFactorNumber);

            if (sellfactor == null)
            {
                throw new SellFactorNotFoundException();
            }

            _repository.Remove(sellfactor);

            var commodity = _commodityRepository
                .GetbyCode(sellfactor.CommodityCode);
            commodity.Inventory += sellfactor.Count;

            _unitOfWork.Commit();
        }

        public IList<GetSellFactorDto> GetAll()
        {
            return _repository.GetAll();
        }

        public GetSellFactorDto GetSellFactor(int sellFactorNumber)
        {
            var sellfactor = _repository.GetSellFactor(sellFactorNumber);

            if (sellfactor == null)
            {
                throw new SellFactorNotFoundException();
            }

            return sellfactor;
        }

        public void Update(int sellFactorNumber, UpdateSellFactorDto dto)
        {
            SellFactor sellfactor = _repository
                .GetBySellFactorNumber(sellFactorNumber);

            if (sellfactor == null)
            {
                throw new SellFactorNotFoundException();
            }

            var commodity = _commodityRepository
                .GetbyCode(sellfactor.CommodityCode);
            commodity.Inventory -= (dto.Count - sellfactor.Count);

            sellfactor.CommodityCode = dto.CommodityCode;
            sellfactor.Date = dto.Date;
            sellfactor.Count = dto.Count;
            sellfactor.BasePrice = dto.BasePrice;
            sellfactor.TotalPrice = dto.TotalPrice;
            sellfactor.BuyerName = dto.BuyerName;

            _unitOfWork.Commit();

            if (commodity.Inventory <= int.Parse(commodity.MinInventory))
            {
                throw new EqualOrLessInventoryThanMinimumCommodityInventoryException();
            }
        }
    }
}
