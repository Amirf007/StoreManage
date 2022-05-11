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
    [Scenario("ویرایش دسته بندی با عنوان تکراری ")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " دسته بندی کالاها را مدیریت کنم  ",
        InOrderTo = "در آن کالاها را تعریف کنم"
    )]
    public class UpdateCategoryWithDuplicateTitle : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private UpdateCategoryDto _dto;
        private Category _category;
        private Category _existcategory;
        Action expected;
        public UpdateCategoryWithDuplicateTitle(ConfigurationFixture configuration)
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Given("دسته بندی کالا با عنوان 'لبنیات' در فهرست دسته بندی های کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory();

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [Given("دسته بندی کالا با عنوان 'شیر و ماست' در فهرست دسته بندی های کالا وجود دارد")]
        public void GivenAnd()
        {
            _existcategory = CategoryFactory.CreateCategory();
            _existcategory.Title = "شیر و ماست";

            _dataContext.Manipulate(_ => _.Categories.Add(_existcategory));
        }

        [When("عنوان دسته بندی با عنوان 'لبنیات' را ب 'شیر و ماست' تغییر میدیم")]
        public void When()
        {
            _dto = UpdateCategoryDtoFactory.GenerateUpdateCategoryDto(_existcategory.Title);

            expected = () => _sut.Update(_category.Id,_dto);
        }

        [Then("تنها یک دسته بندی کالا با عنوان 'شیر و ماست' باید در فهرست دسته بندی کالا وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Categories.Where(_ => _.Title == _dto.Title)
                .Should().HaveCount(1);
        }

        [And("خطایی با عنوان’عنوان دسته بندی کالا تکراریست’ باید رخ دهد")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<DuplicateCategoryTitleException>();
        }

        [Fact]
        public void Run()
        {
            Given();
            GivenAnd();
            When();
            Then();
            ThenAnd();
        }
    }
}
