using FluentAssertions;
using StoreManage.Infrastructure.Application;
using StoreManage.Infrastructure.Test;
using StoreManage.Persistence.EF;
using StoreManage.Persistence.EF.Categories;
using StoreManage.Services.Categories;
using StoreManage.Services.Categories.Contracts;
using StoreManage.Test.Tools.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoreManage.Services.Test.Unit.Categories
{
    public class CategoryServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;

        public CategoryServiceTests()
        {
            _dataContext =
                new EFInMemoryDataBase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_category_properly()
        {
            var dto = new AddCategoryDto
            {
                Title = "لبنیات"
            };

            _sut.Add(dto);

            var expected = _dataContext.Categories
               .FirstOrDefault();
            expected.Title.Should().Be(dto.Title);
        }

        [Fact]
        public void Add_throws_CategoryIsAlreadyExistException_when_category_register_with_duplicate_Title()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var dto = new AddCategoryDto
            {
                Title = category.Title
            };

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<CategoryIsAlreadyExistException>();
        }

        [Fact]
        public void Update_update_category_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            UpdateCategoryDto dto = GenerateUpdateCategoryDto();

            _sut.Update(category.Id, dto);

            var Expected = _dataContext.Categories
                .FirstOrDefault();
            Expected.Title.Should().Be(dto.Title);
        }

        private static UpdateCategoryDto GenerateUpdateCategoryDto()
        {
            return new UpdateCategoryDto
            {
                Title = "شیر و ماست"
            };
        }

        [Fact]
        public void Update_throw_CategoryNotFoundException_when_category_with_given_id_that_not_exists()
        {
            var fakeCategoryId = 100;

            UpdateCategoryDto dto = GenerateUpdateCategoryDto();

            Action Expected = () => _sut.Update(fakeCategoryId, dto);
            Expected.Should().ThrowExactly<CategoryNotFoundException>();
        }

        [Fact]
        public void Update_throw_CategoryIsAlreadyExistException_When_category_update_with_duplicate_Title()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            UpdateCategoryDto dto = GenerateUpdateCategoryDto();
            dto.Title = category.Title;

            Action Expected = () => _sut.Update(category.Id,dto);
            Expected.Should()
                .ThrowExactly<CategoryIsAlreadyExistException>();
        }
    }
}
