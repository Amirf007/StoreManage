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
    [Scenario("حذف ورود کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " ورود کالاها را مدیریت کنم ",
        InOrderTo = "برای هر بار خرید  یک فاکتور خرید داشته باشم و موجودی کالا ها ی خود را افزایش دهم"
    )]
    public class DeleteEntryCommodity : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly BuyFactorService _sut;
        private readonly BuyFactorRepository _buyfactorrepository;
        private readonly CommodityRepository _commodityRepository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;
        private Commodity _commodity;
        private BuyFactor _buyFactor;
        public DeleteEntryCommodity(ConfigurationFixture configuration)
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _buyfactorrepository = new EFBuyFactorRepository(_dataContext);
            _commodityRepository = new EFCommodityRepository(_dataContext);
            _sut = new BuyFactorAppService
                (_buyfactorrepository, _unitOfWork, _commodityRepository);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی های کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory();

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [Given("کالایی با نام 'شیر رامک' و کد '1' با قیمت '150000' ریال و موجودی '10' عدد و بیشترین موجودی '15' عدد و کمترین موجودی '5' عدد در فهرست کالاها در دسته بندی با عنوان 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _commodity = CommodityFactory.CreateCommodity(_category.Id);

            _dataContext.Manipulate(_ => _.Commodities.Add(_commodity));
        }

        [Given("سند ورود کالایی با کد '1' به تعداد '4' عدد در تاریخ '2022/05/08' با قیمت خرید '125000' در فهرست سند های ورود کالا وجود دارد")]
        public void GivenSecondAnd()
        {
            _buyFactor = BuyFactorFactory.GenerateBuyFactor(_commodity.Code);
            _dataContext.Manipulate(_ => _.BuyFactors.Add(_buyFactor));

            _commodity.Inventory += _buyFactor.Count;
        }

        [When("سند ورود کالایی با کد '1' به تعداد '4' عدد در تاریخ '2022/05/08' با قیمت خرید '125000' را حذف میکنم")]
        public void When()
        {
            _sut.Delete(_buyFactor.BuyFactorNumber);
        }

        [Then("هیچ سند خروج کالایی در فهرست سند های خروج کالا نباید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.BuyFactors.Should().HaveCount(0);
        }

        [Then("کالایی با نام 'شیر رامک' و کد '1' و موجودی '10' عدد در  دسته بندی کالا با عنوان 'لبنیات' باید وجود داشته باشد")]
        public void ThenAnd()
        {
            var expected = _dataContext.Commodities.FirstOrDefault();

            expected.Name.Should().Be(_commodity.Name);
            expected.Code.Should().Be(_commodity.Code);
            expected.Inventory.Should().Be(10);
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
