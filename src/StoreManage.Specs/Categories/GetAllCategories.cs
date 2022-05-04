using FluentAssertions;
using StoreManage.Entities;
using StoreManage.Infrastructure.Application;
using StoreManage.Infrastructure.Test;
using StoreManage.Persistence.EF;
using StoreManage.Persistence.EF.Categories;
using StoreManage.Services.Categories;
using StoreManage.Services.Categories.Contracts;
using StoreManage.Specs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static StoreManage.Specs.BDDHelper;

namespace StoreManage.Specs.Categories
{
    [Scenario("مشاهده ی فهرست دسته بندی ها")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " دسته بندی کالاها را مدیریت کنم  ",
        InOrderTo = "در آن کالاها را تعریف کنم"
    )]
    public class GetAllCategories : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private IList<GetCategoryDto> expected;
        public GetAllCategories(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }


        [Given("دسته بندی هایی با عنوان های 'لبنیات' و 'تنقلات' و 'شوینده ها' در فهرست دسته بندی های کالاها وجود دارد")]
        public void Given()
        {
            var categories = new List<Category>
            {
                new Category
                {
                Title = "لبنیات"
                },
                new Category
                {
                Title = "تنقلات"
                },
                new Category
                {
                Title = "شوینده ها"
                }
            };
            _dataContext.Manipulate(_ => _.Categories.AddRange(categories));
        }

        [When("تمام دسته بندی ها را نمایش میدهیم")]
        public void When()
        {
            expected = _sut.GetAll();
        }

        [Then("فهرست دسته بندی های موجود باید شامل 3 دسته بندی کالا باشد")]
        public void Then()
        {
            expected.Should().HaveCount(3);
        }

        [Then("در فهرست دسته بندی کالاها دسته بندی هایی با عنوان های ' لبنیات' و ' تنقلات ' و 'شوینده ها' باید وجود داشته باشد")]
        public void ThenAnd()
        {
            expected.Should().Contain(_ => _.Title == "لبنیات");
            expected.Should().Contain(_ => _.Title == "تنقلات");
            expected.Should().Contain(_ => _.Title == "شوینده ها");
        }

        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
            ThenAnd();
        }
    }
}
