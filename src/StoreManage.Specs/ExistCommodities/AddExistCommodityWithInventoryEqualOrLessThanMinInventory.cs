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
    [Scenario(" تعریف خروج کالا با موجودی برابر یا کمتر از حداقل موجودی ")]
    [Feature("",
       AsA = "فروشنده ",
       IWantTo = " خروج کالاها را مدیریت کنم ",
       InOrderTo = "برای هر فروش کالا یک فاکتور فروش داشته باشم و کالا های خود را بفروشم"
   )]
    public class AddExistCommodityWithInventoryEqualOrLessThanMinInventory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly SellFactorService _sut;
        private readonly SellFactorRepository _sellfactorrepository;
        private readonly CommodityRepository _commodityRepository;
        private readonly UnitOfWork _unitOfWork;
        private AddSellFactorDto _dto;
        private Category _category;
        private Commodity _commodity;
        private int _initialbalance;
        Action expected;
        public AddExistCommodityWithInventoryEqualOrLessThanMinInventory(ConfigurationFixture configuration)
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

        [Given("هیچ سند خروج کالایی در فهرست سند های خروجی کالا وجود ندارد")]
        public void GivenSecondAnd()
        {

        }

        [When("تعداد '5' عدد از کالایی با کد '1'  قیمت پایه '150000' و قیمت کل '450000' در تاریخ '2022/05/08'  خارج میکنم و میفروشم  ")]
        public void When()
        {
            _dto = AddSellFactorDtoFactory
                .GenerateAddSellFactorDto(_commodity.Code);
            _dto.Count = 6;

            expected =()=> _sut.Add(_dto);
        }

        [Then("موجودی کالایی با کد '1' باید بزرگتر از حداقل موجودی کالا با کد '1' باشد ")]
        public void Then()
        {
            var expected = _dataContext.Commodities.FirstOrDefault();

            expected.Inventory.Should()
                .BeGreaterThan(int.Parse(expected.MinInventory));
        }

        [And("خطایی با عنوان 'موجودی کالایی با کد '1' برابر یا کمتر از حداقل موجودی ان است' باید رخ دهد")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly
               <EqualOrLessInventoryThanMinimumCommodityInventoryException>();
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
