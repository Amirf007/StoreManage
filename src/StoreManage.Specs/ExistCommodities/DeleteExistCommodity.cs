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
    [Scenario("حذف خروج کالا ")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = " خروج کالاها را مدیریت کنم ",
      InOrderTo = "برای هر فروش کالا یک فاکتور فروش داشته باشم و کالا های خود را بفروشم"
  )]
    public class DeleteExistCommodity : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly SellFactorService _sut;
        private readonly SellFactorRepository _sellfactorrepository;
        private readonly CommodityRepository _commodityRepository;
        private readonly UnitOfWork _unitOfWork;
        private SellFactor _sellFactor;
        private Category _category;
        private Commodity _commodity;
        private int _initialbalance;
        public DeleteExistCommodity(ConfigurationFixture configuration)
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

            _initialbalance = _commodity.Inventory;
        }

        [Given("سند خروج کالایی با کد '1' به تعداد '3' عدد در تاریخ '2022/05/08' با قیمت پایه '150000' و قیمت کل 450000 در فهرست سند خروجی کالا وجود دارد")]
        public void GivensecondAnd()
        {
            _sellFactor=SellFactorFactory.GenerateSellFactor(_commodity.Code);
            _dataContext.Manipulate(_ => _.SellFactors.Add(_sellFactor));

            _commodity.Inventory -= _sellFactor.Count;
        }

        [When("سند خروج کالایی با کد '1' به تعداد '3' عدد در تاریخ '2022/05/08' با قیمت پایه '150000' و قیمت کل '450000' را حذف میکنم")]
        public void When()
        {
            _sut.Delete(_sellFactor.SellFactorNumber);
        }

        [Then("هیچ سند خروج کالایی در فهرست سند خروج کالا نباید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.SellFactors.Should().HaveCount(0);
        }

        [Then("کالایی با نام 'شیر رامک' و کد '1' و موجودی '10' عدد در  دسته بندی کالا با عنوان 'لبنیات' باید وجود داشته باشد")]
        public void ThenAnd()
        {
            var expected = _dataContext.Commodities.FirstOrDefault();

            expected.Name.Should().Be(_commodity.Name);
            expected.Code.Should().Be(_commodity.Code);
            expected.Inventory.Should().Be(_initialbalance);
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
