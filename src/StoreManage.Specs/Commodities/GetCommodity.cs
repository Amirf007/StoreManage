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
    [Scenario("مشاهده کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " کالاها را مدیریت کنم ",
        InOrderTo = "کالا ها را دسته بندی و خرید و فروش کنم"
    )]
    public class GetCommodity : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CommodityService _sut;
        private readonly CommodityRepository _repository;
        private readonly CategoryRepository _categoryRepository;
        private readonly UnitOfWork _unitOfWork;
        private GetCommodityDto expected;
        private Commodity _commodity;
        private Category _category;
        public GetCommodity(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCommodityRepository(_dataContext);
            _sut = new CommodityAppService(_repository, _unitOfWork, _categoryRepository);
        }

        [Given("دسته بندی با عنوان 'لبنیات'در فهرست دسته بندی کالاها وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory();

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [Given("کالایی با نام 'ماست رامک' با قیمت '150000' ریال و موجودی '10' عدد و بیشترین موجودی '15' عدد و کمترین موجودی '5' عدد در فهرست کالاها در دسته بندی با عنوان 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _commodity = CommodityFactory.CreateCommodity(_category.Id);

            _dataContext.Manipulate(_ => _.Commodities.Add(_commodity));
        }

        [When("درخواست نمایش کالایی در دسته بندی با عنوان 'لبنیات' رو میدم")]
        public void When()
        {
            expected = _sut.GetCommodity(_commodity.Code);
        }

        [Then("کالایی با نام 'ماست رامک' و قیمت '150000' ریال و موجودی '10' عدد و بیشترین موجودی '15' عدد و کمترین موجودی '5' عدد در  دسته بندی کالا با عنوان 'لبنیات' باید وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.Commodities.FirstOrDefault();

            expected.Name.Should().Be(_commodity.Name);
            expected.Price.Should().Be(_commodity.Price);
            expected.Inventory.Should().Be(_commodity.Inventory);
            expected.MaxInventory.Should().Be(_commodity.MaxInventory);
            expected.MinInventory.Should().Be(_commodity.MinInventory);
            expected.Category.Id.Should().Be(_commodity.CategoryId);
        }

        [Fact]
        public void Run()
        {
            Given();
            GivenAnd();
            When();
            Then();
        }
    }
}
