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
    [Scenario("تعریف خروج کالا ")]
    [Feature("",
       AsA = "فروشنده ",
       IWantTo = " خروج کالاها را مدیریت کنم ",
       InOrderTo = "برای هر فروش کالا یک فاکتور فروش داشته باشم و کالا های خود را بفروشم"
   )]
    public class AddExistCommodity : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly SellFactorService _sut;
        private readonly SellFactorRepository _sellfactorrepository;
        private readonly CommodityRepository _commodityRepository;
        private readonly UnitOfWork _unitOfWork;
        private AddSellFactorDto _dto;
        private Category _category;
        private Commodity _commodity;
        public AddExistCommodity(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _sellfactorrepository = new EFSellFactorRepository(_dataContext);
            _commodityRepository = new EFCommodityRepository(_dataContext);
            _sut = new SellFactorAppService(_sellfactorrepository, _unitOfWork, _commodityRepository);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی کالاها وجود دارد")]
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

        [Given("هیچ سند خروج کالایی در فهرست سند خروجی کالا وجود ندارد")]
        public void GivenAnd2()
        {

        }

        [When("تعداد '3' عدد از کالایی با کد '1'  قیمت پایه '150000' و قیمت کل '450000' در تاریخ '19 / 02 / 1400'  میفروشم  ")]
        public void When()
        {
            _dto = AddSellFactorDtoFactory.GenerateAddSellFactorDto(_commodity.Code);

            _sut.Add(_dto);
        }

        [Then("سند خروج کالایی با کد '100' با تعداد '10' در تاریخ '21 / 02 / 1400' در فهرست سند خروجی کالا باید وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.SellFactors.FirstOrDefault();
            expected.CommodityCode.Should().Be(_commodity.Code);
            expected.Date.Should().Be(_dto.Date);
            expected.BasePrice.Should().Be(_dto.BasePrice);
            expected.TotalPrice.Should().Be(_dto.TotalPrice);
            expected.Count.Should().Be(_dto.Count);
            expected.BuyerName.Should().Be(_dto.BuyerName);
        }

        [Then("کالایی با نام 'شیر رامک' و کد '1' و موجودی '7' عدد در  دسته بندی کالا با عنوان 'لبنیات' باید وجود داشته باشد")]
        public void ThenAnd()
        {
            var expected = _dataContext.Commodities.FirstOrDefault();

            expected.Name.Should().Be(_commodity.Name);
            expected.Code.Should().Be(_dto.CommodityCode);
            expected.Inventory.Should().Be(7);
        }

        [Fact]
        public void Run()
        {
            Given();
            GivenAnd();
            GivenAnd2();
            When();
            Then();
            ThenAnd();
        }
    }
}
