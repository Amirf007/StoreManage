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
    [Scenario("تعریف کالا ")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " کالاها را مدیریت کنم ",
        InOrderTo = "کالا ها را دسته بندی و خرید و فروش کنم"
    )]
    public class AddCommodity : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CommodityService _sut;
        private readonly CommodityRepository _commodityrepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly UnitOfWork _unitOfWork;
        private AddCommodityDto _dto;
        private Category _category;
        public AddCommodity(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _commodityrepository = new EFCommodityRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CommodityAppService(_commodityrepository, _unitOfWork,_categoryRepository);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory();

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [Given("هیچ کالایی در دسته بندی با عنوان 'لبنیات' وجود ندارد")]
        public void GivenAnd()
        {
        }

        [When("کالایی با نام 'شیر رامک' و قیمت '150000' ریال و موجودی '10' عدد و بیشترین موجودی '15' عدد و کمترین موجودی '5' عدد در دسته بندی با عنوان 'لبنیات' تعریف میکنم")]
        public void When()
        {
            _dto = AddCommodityDtoFactory.GenerateAddCommodityDto(_category.Id);

            _sut.Add(_dto);
        }

        [Then("کالایی با نام 'شیر رامک' و قیمت '150000' ریال و موجودی '10' عدد و بیشترین موجودی '15' عدد و کمترین موجودی '5' عدد در  دسته بندی کالا با عنوان 'لبنیات' باید وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.Commodities.FirstOrDefault();

            expected.Name.Should().Be(_dto.Name);
            expected.Price.Should().Be(_dto.Price);
            expected.Inventory.Should().Be(_dto.Inventory);
            expected.MaxInventory.Should().Be(_dto.MaxInventory);
            expected.MinInventory.Should().Be(_dto.MinInventory);
            expected.Category.Id.Should().Be(_dto.CategoryId);
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
