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
        private CommodityRepository repository;
        private UnitOfWork unitOfWork;
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
            var isNameDuplicate = _repository.IsExistNameCommodity(dto.Name , dto.CategoryId);

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

        public void Delete(int code)
        {
            var commodity = _repository.GetbyId(code);
            if (commodity==null)
            {
                throw new CommodityNotFoundException();
            }

            _repository.Remove(commodity);

            _unitOfWork.Commit();
        }

        public IList<GetCommodityDto> GetAll()
        {
            return _repository.GetAll();
        }

        public GetCommodityDto GetCommodity(int code)
        {
            var commodity = _repository.GetCommodity(code);

            if (commodity==null)
            {
                throw new CommodityNotFoundException();
            }

            return commodity;
        }

        public void Update(int code, UpdateCommodityDto dto)
        {
            var commodity = _repository.GetbyId(code);
            if (commodity == null)
            {
                throw new CommodityNotFoundException();
            }

            var IsExist = _repository
                .IsExistName(dto.Name,dto.CategoryId,commodity.Code);
            if (IsExist)
            {
                throw new DuplicateCommodityNameInCategoryException();
            }

            commodity.Name = dto.Name;
            commodity.Price = dto.Price;
            commodity.Inventory = dto.Inventory;
            commodity.MaxInventory = dto.MaxInventory;
            commodity.MinInventory = dto.MinInventory;
            commodity.CategoryId = dto.CategoryId;

            _unitOfWork.Commit();
        }
    }
}
