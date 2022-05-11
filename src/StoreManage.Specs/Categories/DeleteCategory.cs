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
    [Scenario("حذف دسته بندی ")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " دسته بندی کالاها را مدیریت کنم  ",
        InOrderTo = "در آن کالاها را تعریف کنم"
    )]
    public class DeleteCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;
        public DeleteCategory(ConfigurationFixture configuration) 
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory();

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [When("دسته بندی با عنوان 'لبنیات' را حذف میکنیم")]
        public void When()
        { 
            _sut.Delete(_category.Id);
        }

        [Then("هیچ دسته بندی ای در فهرست دسته بندی های کالا نباید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Categories.Should().HaveCount(0);
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
