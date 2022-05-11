using FluentAssertions;
using StoreManage.Entities;
using StoreManage.Infrastructure.Application;
using StoreManage.Infrastructure.Test;
using StoreManage.Persistence.EF;
using StoreManage.Persistence.EF.Commodities;
using StoreManage.Persistence.EF.SellFactors;
using StoreManage.Services.Commodities.Contracts;
using StoreManage.Services.SellFactors;
using StoreManage.Services.SellFactors.Contracts;
using StoreManage.Specs.Infrastructure;
using StoreManage.Test.Tools.Categories;
using StoreManage.Test.Tools.Commodities;
using StoreManage.Test.Tools.SellFactors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static StoreManage.Specs.BDDHelper;

namespace StoreManage.Specs.ExistCommodities
{
    [Scenario("مشاهده فهرست خروجی های کالا ")]
    [Feature("",
       AsA = "فروشنده ",
       IWantTo = " خروج کالاها را مدیریت کنم ",
       InOrderTo = "برای هر فروش کالا یک فاکتور فروش داشته باشم و کالا های خود را بفروشم"
   )]
    public class GetAllExistCommodities : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly SellFactorService _sut;
        private readonly SellFactorRepository _sellfactorrepository;
        private readonly CommodityRepository _commodityRepository;
        private readonly UnitOfWork _unitOfWork;
        private SellFactor _sellFactor;
        private SellFactor _secondsellFactor;
        private Category _category;
        private Commodity _commodity;
        private IList<GetSellFactorDto> expected;
        public GetAllExistCommodities(ConfigurationFixture configuration)
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _sellfactorrepository = new EFSellFactorRepository(_dataContext);
            _commodityRepository = new EFCommodityRepository(_dataContext);
            _sut = new SellFactorAppService
                (_sellfactorrepository, _unitOfWork, _commodityRepository);
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

        [Given("سند های خروج کالایی با کد '1' و تعداد '3' و '1' عدد و قیمت پایه '150000' و قیمت کل '450000' و '150000' در تاریخ '2022/05/08' در فهرست سند های خروج کالا وجود دارند")]
        public void GivensecondAnd()
        {
            _sellFactor=SellFactorFactory.GenerateSellFactor(_commodity.Code);
            _dataContext.Manipulate(_ => _.SellFactors.Add(_sellFactor));

            _secondsellFactor = SellFactorFactory
                .GenerateSellFactor(_commodity.Code);
            _secondsellFactor.Count = 1;
            _secondsellFactor.TotalPrice = "150000";
            _dataContext.Manipulate(_ => _.SellFactors.Add(_secondsellFactor));

            _commodity.Inventory -= _sellFactor.Count + _secondsellFactor.Count;
        }

        [When("درخواست نمایش فهرست سند های خروج کالا را میدهم")]
        public void When()
        {
            expected = _sut.GetAll();
        }

        [Then("فهرست خروجی های کالا ی موجود باید شامل 3 خروج کالا باشد ")]
        public void Then()
        {
            expected.Should().HaveCount(2);
        }

        [Then("سند های خروج کالایی با کد '1' و تعداد '3' و '1' عدد و قیمت پایه '150000' و قیمت کل '450000' و '150000' در تاریخ '2022/05/08' در فهرست سند های خروج کالا باید وجود داشته باشند ")]
        public void ThenAnd()
        {
            expected.Should().Contain(_ => _.CommodityCode == _commodity.Code 
            && _.Count == _sellFactor.Count && _.Date == _sellFactor.Date
            && _.BasePrice == _sellFactor.BasePrice
            && _.TotalPrice == _sellFactor.TotalPrice 
            && _.BuyerName == _sellFactor.BuyerName);

            expected.Should().Contain(_ => _.CommodityCode == _commodity.Code
            && _.Count == _secondsellFactor.Count 
            && _.Date == _secondsellFactor.Date
            && _.BasePrice == _secondsellFactor.BasePrice
            && _.TotalPrice == _secondsellFactor.TotalPrice
            && _.BuyerName == _secondsellFactor.BuyerName);
        }

        [Fact]
        public void Run()
        {
            Given();
            GivenAnd();
            GivensecondAnd();
            When();
            Then();
            ThenAnd();
        }
    }
}
