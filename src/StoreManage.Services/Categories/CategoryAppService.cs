using StoreManage.Entities;
using StoreManage.Infrastructure.Application;
using StoreManage.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManage.Services.Categories
{
    public class CategoryAppService : CategoryService
    {
        private CategoryRepository _repository;
        private UnitOfWork _unitOfWork;

        public CategoryAppService
            (CategoryRepository repository, UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddCategoryDto dto)
        {
            var isTitleDuplicate = _repository.IsExistCategoryTitle(dto.Title);

            if (isTitleDuplicate)
            {
                throw new DuplicateCategoryTitleException();
            }

            var category = new Category
            {
                Title = dto.Title
            };

            _repository.Add(category);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var category = _repository.GetbyId(id);

            if (category==null)
            {
                throw new CategoryNotFoundException();
            }

            _repository.Remove(category);

            _unitOfWork.Commit();
        }

        public IList<GetCategoryDto> GetAll()
        {
           return _repository.GetAll();
        }

        public void Update(int id, UpdateCategoryDto dto)
        {
            Category category = _repository.GetbyId(id);

            if (category==null)
            {
                throw new CategoryNotFoundException();
            }

            var IsExistCategory = _repository
                .IsExistCategoryTitle(dto.Title);
            if (IsExistCategory)
            {
                throw new DuplicateCategoryTitleException();
            }

            category.Title = dto.Title;

            _unitOfWork.Commit();
        }
    }
}
