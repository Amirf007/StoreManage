﻿using StoreManage.Entities;
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

        public Commodity GetbyId(int code)
        {
            return _dataContext.Commodities.Find(code);
        }

        public bool IsExistName(string name,int CategoryId, int code)
        {
            return _dataContext.Commodities
               .Any(_ => _.Name == name 
               && _.CategoryId == CategoryId && _.Code != code);
        }

        public bool IsExistNameCommodity(string name,int CategoryId)
        {
            return _dataContext.Commodities.Any(_ => _.Name==name && _.CategoryId == CategoryId);
        }
    }
}
