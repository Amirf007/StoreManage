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
    [Scenario("ویرایش کالا با کد تکرای در یک دسته بندی")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " کالاها را مدیریت کنم ",
        InOrderTo = "کالا ها را دسته بندی و خرید و فروش کنم"
    )]
    public class UpdateCommodityWithDuplicateCode : EFDataContextDatabaseFixture
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
        public UpdateCommodityWithDuplicateCode(ConfigurationFixture configuration)
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _commodityrepository = new EFCommodityRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CommodityAppService
                (_commodityrepository, _unitOfWork, _categoryRepository);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی های کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory();

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [Given("کالایی با کد '1' و نام 'شیر رامک' و قیمت '150000' ریال و موجودی '10' عدد و بیشترین موجودی '15' عدد و کمترین موجودی '5' عدد در دسته بندی با عنوان 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _commodity = CommodityFactory.CreateCommodity(_category.Id);

            _dataContext.Manipulate(_ => _.Commodities.Add(_commodity));
        }

        [Given(" کالایی با کد '2' در دسته بندی با عنوان 'لبنیات' وجود دارد")]
        public void GivenSecondAnd()
        {
            _existcommodity = CommodityFactory.CreateCommodity(_category.Id);
            _existcommodity.Code = 2;

            _dataContext.Manipulate(_ => _.Commodities.Add(_existcommodity));
        }

        [When("نام و کد کالایی با کد '1' و نام 'شیر رامک' و قیمت '150000' ریال و موجودی '10' عدد و بیشترین موجودی '15' و کمترین موجودی '5' را ب  'شیر پر چرب رامک' با کد '2' تغییر میدهم")]
        public void When()
        {
            _dto = UpdateCommodityDtoFactory
                .GenerateUpdateCommodityDto(_category.Id);
            _dto.Name = "شیر پر چرب رامک";
            _dto.Code = _existcommodity.Code;

            expected = () => _sut.Update(_commodity.Code, _dto);
        }

        [Then("در فهرست کالاها تنها یک کالا با کد '1' باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Commodities.Where(_ => _.Code == _commodity.Code)
                .Should().HaveCount(1);
        }

        [And("خطایی با عنوان 'کد کالا تکراریست ' باید رخ دهد")]
        public void ThenAnd()
        {
            expected.Should()
                .ThrowExactly<DuplicateCommodityCodeException>();
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
