using StoreManage.Entities;
using StoreManage.Infrastructure.Application;
using StoreManage.Infrastructure.Test;
using StoreManage.Persistence.EF;
using StoreManage.Persistence.EF.BuyFactors;
using StoreManage.Services.BuyFactors;
using StoreManage.Services.BuyFactors.Contracts;
using StoreManage.Specs.Infrastructure;
using StoreManage.Test.Tools.Categories;
using StoreManage.Test.Tools.Commodities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StoreManage.Specs.BDDHelper;

namespace StoreManage.Specs.EntryCommodity
{
    [Scenario("تعریف ورود کالا ")]
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
        private readonly UnitOfWork _unitOfWork;
        private AddBuyFactorDto _dto;
        private Category _category;
        private Commodity _commodity;
        public AddEntryCommodity(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _buyfactorrepository = new EFBuyFactorRepository(_dataContext);
            _sut = new BuyFactorAppService(_buyfactorrepository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی کالاها وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory();

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [Given("کالایی با نام 'شیر رامک' با قیمت '150000' ریال و موجودی '10' عدد و بیشترین موجودی '15' عدد و کمترین موجودی '5' عدد در فهرست کالاها در دسته بندی با عنوان 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _commodity = CommodityFactory.CreateCommodity(_category.Id);

            _dataContext.Manipulate(_ => _.Commodities.Add(_commodity));
        }

        [Given("هیچ سند ورود کالایی در فهرست سند ورودی کالا وجود ندارد")]
        public void GivenAnd2()
        {
            
        }

        ////[When("کالایی با نام 'شیر رامک' و قیمت '150000' ریال و موجودی '10' عدد و بیشترین موجودی '15' عدد و کمترین موجودی '5' عدد در دسته بندی با عنوان 'لبنیات' تعریف میکنم")]
        ////public void When()
        ////{
        ////    var dto = new AddBuyFactorDto
        ////    {

        ////    };

        ////    _sut.Add(_dto);
        ////}
    }
}
