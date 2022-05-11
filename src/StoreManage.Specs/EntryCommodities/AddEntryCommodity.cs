using FluentAssertions;
using StoreManage.Entities;
using StoreManage.Infrastructure.Application;
using StoreManage.Infrastructure.Test;
using StoreManage.Persistence.EF;
using StoreManage.Persistence.EF.BuyFactors;
using StoreManage.Persistence.EF.Commodities;
using StoreManage.Services.BuyFactors;
using StoreManage.Services.BuyFactors.Contracts;
using StoreManage.Services.Commodities.Contracts;
using StoreManage.Specs.Infrastructure;
using StoreManage.Test.Tools.BuyFactors;
using StoreManage.Test.Tools.Categories;
using StoreManage.Test.Tools.Commodities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static StoreManage.Specs.BDDHelper;

namespace StoreManage.Specs.EntryCommodity
{
    [Scenario(" تعریف ورود کالا ")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " ورود کالاها را مدیریت کنم ",
        InOrderTo = "برای هر بار خرید  یک فاکتور خرید داشته باشم و موجودی کالا ها ی خود را افزایش دهم"
    )]
    public class AddEntryCommodity : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly BuyFactorService _sut;
        private readonly BuyFactorRepository _buyfactorrepository;
        private readonly CommodityRepository _commodityRepository;
        private readonly UnitOfWork _unitOfWork;
        private AddBuyFactorDto _dto;
        private Category _category;
        private Commodity _commodity;
        private int _initialbalance;
        public AddEntryCommodity(ConfigurationFixture configuration)
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _buyfactorrepository = new EFBuyFactorRepository(_dataContext);
            _commodityRepository = new EFCommodityRepository(_dataContext);
            _sut = new BuyFactorAppService
                (_buyfactorrepository, _unitOfWork , _commodityRepository);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی های کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory();

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [Given("کالایی با کد '1' و نام 'شیر رامک' و کد '1' با قیمت '150000' ریال و موجودی '10' عدد و بیشترین موجودی '15' عدد و کمترین موجودی '5' عدد در فهرست کالاها در دسته بندی با عنوان 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _commodity = CommodityFactory.CreateCommodity(_category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(_commodity));

            _initialbalance = _commodity.Inventory;
        }

        [Given("هیچ سند ورود کالایی در فهرست سند های ورودی کالا وجود ندارد")]
        public void GivenSecondAnd()
        {
            
        }

        [When("کالایی با کد '1' و تعداد '4' و قیمت خرید '125000' در تاریخ '2022/05/08'  وارد میکنم")]
        public void When()
        {
            _dto = AddBuyFactorDtoFactory
                .GenerateAddBuyFactorDto(_commodity.Code);

            _sut.Add(_dto);
        }

        [Then("سند ورود کالایی با کد '1' با تعداد '4' در تاریخ '2022/05/08' در فهرست سند های ورودی کالا باید وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.BuyFactors.FirstOrDefault();
            expected.CommodityCode.Should().Be(_commodity.Code);
            expected.Date.Should().Be(_dto.Date);
            expected.BuyPrice.Should().Be(_dto.BuyPrice);
            expected.Count.Should().Be(_dto.Count);
            expected.SellerName.Should().Be(_dto.SellerName);
        }

        [Then("کالایی با نام 'شیر رامک' و کد '1' و موجودی '14' عدد در  دسته بندی کالا با عنوان 'لبنیات' باید وجود داشته باشد")]
        public void ThenAnd()
        {
            var expected = _dataContext.Commodities.FirstOrDefault();

            expected.Name.Should().Be(_commodity.Name);
            expected.Code.Should().Be(_dto.CommodityCode);
            expected.Inventory.Should().Be(_initialbalance + _dto.Count);
        }

        [Fact]
        public void Run()
        {
            Given();
            GivenAnd();
            GivenSecondAnd();
            When();
            Then();
            ThenAnd();
        }
    }
}
