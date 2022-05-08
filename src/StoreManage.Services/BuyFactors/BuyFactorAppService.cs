﻿using StoreManage.Entities;
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
    }
}
