using StoreManage.Entities;
using StoreManage.Infrastructure.Application;
using StoreManage.Services.Categories.Contracts;
using StoreManage.Services.Commodities.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Services.Commodities
{
    public class CommodityAppService : CommodityService
    {
        private readonly CommodityRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;

        public CommodityAppService(
            CommodityRepository repositoy,
            UnitOfWork unitOfWork,
            CategoryRepository cateogryRepository)
        {
            _unitOfWork = unitOfWork;
            _repository = repositoy;
            _categoryRepository = cateogryRepository;
        }

        public void Add(AddCommodityDto dto)        
        {
            var isNameDuplicate = _repository.IsExistNameCommodity(dto.Name);

            if (isNameDuplicate)
            {
                throw new DuplicateCommodityNameInCategoryException();
            }

            var commodity = new Commodity
            {
                Name = dto.Name,
                Price = dto.Price,
                Inventory = dto.Inventory,
                MaxInventory = dto.MaxInventory,
                MinInventory = dto.MinInventory,
                CategoryId = dto.CategoryId,
            };

            _repository.Add(commodity);

            _unitOfWork.Commit();
        }
    }
}
