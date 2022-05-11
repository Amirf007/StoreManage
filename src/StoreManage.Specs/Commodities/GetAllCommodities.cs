using FluentAssertions;
using StoreManage.Entities;
using StoreManage.Infrastructure.Application;
using StoreManage.Infrastructure.Test;
using StoreManage.Persistence.EF;
using StoreManage.Persistence.EF.Commodities;
using StoreManage.Services.Categories.Contracts;
using StoreManage.Services.Commodities;
using StoreManage.Services.Commodities.Contracts;
using StoreManage.Specs.Infrastructure;
using StoreManage.Test.Tools.Categories;
using StoreManage.Test.Tools.Commodities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static StoreManage.Specs.BDDHelper;

namespace StoreManage.Specs.Commodities
{
    [Scenario("مشاهده فهرست کالاها")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " کالاها را مدیریت کنم ",
        InOrderTo = "کالا ها را دسته بندی و خرید و فروش کنم"
    )]
    public class GetAllCommodities : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CommodityService _sut;
        private readonly CommodityRepository _repository;
        private readonly CategoryRepository _categoryRepository;
        private readonly UnitOfWork _unitOfWork;
        private IList<GetCommodityDto> expected;
        private Category _category;
        public GetAllCommodities(ConfigurationFixture configuration)
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCommodityRepository(_dataContext);
            _sut = new CommodityAppService
                (_repository, _unitOfWork ,_categoryRepository);
        }

        [Given("دسته بندی با عنوان 'لبنیات'در فهرست دسته بندی های کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory();

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [Given("کالاهایی با کد های '1' و '2' و نام های 'شیر رامک' و 'دوغ رامک' با قیمت های '150000' و '120000' و موجودی های '10' عدد و '15' عدد و بیشترین موجودی های '15' عدد و '20' عدد و کمترین موجودی های '5' عدد و '7' عدد در فهرست کالاها در دسته بندی با عنوان 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            var commodities = CommoditiesFactory
                .GenerateCommodities(_category.Id);

            _dataContext.Manipulate(_ => _.Commodities.AddRange(commodities));
        }

        [When("درخواست نمایش فهرست کالاها را میدهم")]
        public void When()
        {
            expected = _sut.GetAll();
        }

        [Then("فهرست کالا های موجود  باید شامل 2 کالا  باشد")]
        public void Then()
        {
            expected.Should().HaveCount(2);
        }

        [Then("در فهرست کالاها  کالاهایی با کد های '1' و '2' و نام های 'شیر رامک' و 'دوغ رامک' با قیمت های '150000' و '120000' و موجودی های '10' و '15' و بیشترین موجودی های '15' و '20' و کمترین موجودی های '5' و '7'  در دسته بندی با عنوان 'لبنیات' باید وجود داشته باشد")]
        public void ThenAnd()
        {
            expected.Should().Contain(_ => _.Name == "شیر رامک"
            && _.Price == "150000" && _.Inventory == 10 && _.MaxInventory=="15"
            && _.MinInventory == "5" && _.CategoryId == _category.Id);

            expected.Should().Contain(_ => _.Name == "دوغ رامک" 
            && _.Price == "120000" && _.Inventory == 15 && _.MaxInventory=="20"
            && _.MinInventory == "7" && _.CategoryId == _category.Id);
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
