using StoreManage.Entities;
using StoreManage.Infrastructure.Application;
using StoreManage.Services.BuyFactors.Contracts;
using StoreManage.Services.Commodities.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Services.BuyFactors
{
    public class BuyFactorAppService : BuyFactorService
    {
        private BuyFactorRepository _repository;
        private UnitOfWork _unitOfWork;
        private CommodityRepository _commodityRepository;

        public BuyFactorAppService(BuyFactorRepository buyfactorrepository, UnitOfWork unitOfWork , CommodityRepository commodityRepository)
        {
            _repository = buyfactorrepository;
            _unitOfWork = unitOfWork;
            _commodityRepository = commodityRepository;
        }

        public void Add(AddBuyFactorDto dto)
        {
            var buyfactor = new BuyFactor
            {
                CommodityCode = dto.CommodityCode,
                Date = dto.Date,
                Count = dto.Count,
                BuyPrice = dto.BuyPrice,
                SellerName = dto.SellerName,
            };

            _repository.Add(buyfactor);

            Commodity commodity = _commodityRepository.GetbyId(buyfactor.CommodityCode);
            commodity.Inventory += buyfactor.Count;

            _unitOfWork.Commit();
        }

        public void Delete(int buyFactorNumber)
        {
            var buyfactor = _repository.GetbyFactorNumber(buyFactorNumber);
            if (buyfactor == null)
            {
                throw new BuyFactorNotFoundException();
            }

            _repository.Remove(buyfactor);

            var commodity = _commodityRepository.GetbyId(buyfactor.CommodityCode);
            commodity.Inventory -= buyfactor.Count;

            _unitOfWork.Commit();
        }

        public IList<GetBuyFactorDto> GetAll()
        {
            return _repository.GetAll();
        }

        public GetBuyFactorDto GetBuyFactor(int buyFactorNumber)
        {
            var buyfactor = _repository.GetBuyFactor(buyFactorNumber);
            if (buyfactor == null)
            {
                throw new BuyFactorNotFoundException();
            }

            return buyfactor;
        }

        public void Update(int buyFactorNumber, UpdateBuyFactorDto dto)
        {
            var buyfactor = _repository.GetbyFactorNumber(buyFactorNumber);
            if (buyfactor == null)
            {
                throw new BuyFactorNotFoundException();
            }

            var commodity = _commodityRepository.GetbyId(buyfactor.CommodityCode);
            commodity.Inventory += (dto.Count - buyfactor.Count);

            buyfactor.CommodityCode = dto.CommodityCode;
            buyfactor.Date = dto.Date;
            buyfactor.Count = dto.Count;
            buyfactor.BuyPrice = dto.BuyPrice;
            buyfactor.SellerName = dto.SellerName;

            _unitOfWork.Commit();
        }
    }
}
