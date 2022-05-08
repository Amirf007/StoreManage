using FluentAssertions;
using StoreManage.Entities;
using StoreManage.Infrastructure.Application;
using StoreManage.Infrastructure.Test;
using StoreManage.Persistence.EF;
using StoreManage.Persistence.EF.Categories;
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
    [Scenario("ویرایش کالا با عنوان تکرای در یک دسته بندی")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " کالاها را مدیریت کنم ",
        InOrderTo = "کالا ها را دسته بندی و خرید و فروش کنم"
    )]
    public class UpdateCommodityWithDuplicateName : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CommodityService _sut;
        private readonly CommodityRepository _commodityrepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly UnitOfWork _unitOfWork;
        private UpdateCommodityDto _dto;
        private Category _category;
        private Commodity _commodity;
        private Commodity _existcommodity;
        Action expected;
        public UpdateCommodityWithDuplicateName(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _commodityrepository = new EFCommodityRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CommodityAppService(_commodityrepository, _unitOfWork, _categoryRepository);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی کالاها وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory();

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [Given("کالایی با نام 'شیر رامک' و قیمت '150000' ریال و موجودی '10' عدد و بیشترین موجودی '15' عدد و کمترین موجودی '5' عدد در دسته بندی با عنوان 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _commodity = CommodityFactory.CreateCommodity(_category.Id);

            _dataContext.Manipulate(_ => _.Commodities.Add(_commodity));
        }

        [Given("و : کالایی با نام 'شیر پر چرب رامک' و قیمت '170000' ریال و موجودی '9' عدد و بیشترین موجودی '15' عدد و کمترین موجودی '5' عدد در دسته بندی با عنوان 'لبنیات' وجود دارد")]
        public void GivenAnd2()
        {
            _existcommodity = CommodityFactory.CreateCommodity(_category.Id);
            _existcommodity.Code = 2;
            _existcommodity.Name = "شیر پر چرب رامک";
            _existcommodity.Price = "170000";
            _existcommodity.Inventory = 9;

            _dataContext.Manipulate(_ => _.Commodities.Add(_existcommodity));
        }

        [When("نام کالایی با نام 'شیر رامک' و قیمت '150000' ریال و موجودی '10' عدد و بیشترین موجودی '15' و کمترین موجودی '5' را ب  'شیر پر چرب رامک' تغییر میدم")]
        public void When()
        {
            _dto = UpdateCommodityDtoFactory.GenerateUpdateCommodityDto(_category.Id);
            _dto.Name = _existcommodity.Name;

            expected = () => _sut.Update(_commodity.Code, _dto);
        }

        [Then("در فهرست کالاها تنها یک کالا باید با نام 'شیر پر چرب رامک ' در دسته بندی با عنوان 'لبنیات' وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Commodities.Where(_ => _.Name == _dto.Name && _.CategoryId == _category.Id && _.Code != _commodity.Code)
                .Should().HaveCount(1);
        }

        [And("خطایی با عنوان 'نام کالا تکراریست ' باید رخ دهد")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<DuplicateCommodityNameInCategoryException>();
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
