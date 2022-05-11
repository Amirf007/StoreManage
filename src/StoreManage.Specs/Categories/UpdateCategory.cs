using FluentAssertions;
using StoreManage.Entities;
using StoreManage.Infrastructure.Application;
using StoreManage.Infrastructure.Test;
using StoreManage.Persistence.EF;
using StoreManage.Persistence.EF.Categories;
using StoreManage.Services.Categories;
using StoreManage.Services.Categories.Contracts;
using StoreManage.Specs.Infrastructure;
using StoreManage.Test.Tools.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static StoreManage.Specs.BDDHelper;

namespace StoreManage.Specs.Categories
{
    [Scenario(" ویرایش دسته بندی کالا ")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " دسته بندی کالاها را مدیریت کنم  ",
        InOrderTo = "در آن کالاها را تعریف کنم"
    )]
    public class UpdateCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private UpdateCategoryDto _dto;
        private Category _category;
        public UpdateCategory(ConfigurationFixture configuration)
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی های کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory();

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [When("عنوان دسته بندی با عنوان 'لبنیات' را ب  'شیر و ماست' تغییر میدهم")]
        public void When()
        {
            _dto = UpdateCategoryDtoFactory.GenerateUpdateCategoryDto("شیر و ماست");

            _sut.Update(_category.Id,_dto);
        }

        [Then("دسته بندی کالا  با عنوان 'شیر و ماست' در فهرست دسته بندی های کالا باید وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.Categories.FirstOrDefault();

            expected.Title.Should().Be(_dto.Title);
        }

        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }
    }
}
