using FluentAssertions;
using StoreManage.Infrastructure.Application;
using StoreManage.Infrastructure.Test;
using StoreManage.Persistence.EF;
using StoreManage.Persistence.EF.BuyFactors;
using StoreManage.Persistence.EF.Commodities;
using StoreManage.Services.BuyFactors;
using StoreManage.Services.BuyFactors.Contracts;
using StoreManage.Services.Commodities.Contracts;
using StoreManage.Test.Tools.BuyFactors;
using StoreManage.Test.Tools.Categories;
using StoreManage.Test.Tools.Commodities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoreManage.Services.Test.Unit.BuyFactors
{
    public class BuyFactorServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly BuyFactorService _sut;
        private readonly BuyFactorRepository _repository;
        private readonly CommodityRepository _commodityRepository;

        public BuyFactorServiceTests()
        {
            _dataContext =
                new EFInMemoryDataBase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFBuyFactorRepository(_dataContext);
            _commodityRepository = new EFCommodityRepository(_dataContext);
            _sut = new BuyFactorAppService
                (_repository, _unitOfWork, _commodityRepository);
        }

        [Fact]
        public void Add_increase_inventory_of_commodity_that_entry_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));
            var initialbalance = commodity.Inventory;

            AddBuyFactorDto dto = AddBuyFactorDtoFactory
                .GenerateAddBuyFactorDto(commodity.Code);

            _sut.Add(dto);

            var expected = _dataContext.Commodities.FirstOrDefault();
            expected.Name.Should().Be(commodity.Name);
            expected.Code.Should().Be(commodity.Code);
            expected.Inventory.Should().Be(initialbalance + dto.Count);
        }

        [Fact]
        public void Add_adds_buyfactor_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            AddBuyFactorDto dto = AddBuyFactorDtoFactory
                .GenerateAddBuyFactorDto(commodity.Code);

            _sut.Add(dto);

            var expected = _dataContext.BuyFactors.FirstOrDefault();
            expected.CommodityCode.Should().Be(commodity.Code);
            expected.Date.Should().Be(dto.Date);
            expected.BuyPrice.Should().Be(dto.BuyPrice);
            expected.Count.Should().Be(dto.Count);
            expected.SellerName.Should().Be(dto.SellerName);
        }

        [Fact]
        public void Update_update_inventory_of_entrycommodity_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            var buyFactor = BuyFactorFactory.GenerateBuyFactor(commodity.Code);
            _dataContext.Manipulate(_ => _.BuyFactors.Add(buyFactor));
            commodity.Inventory += buyFactor.Count;

            var dto = UpdateBuyFactorDtoFactory
                .GenerateUpdateBuyFactorDto(commodity.Code);

            _sut.Update(buyFactor.BuyFactorNumber, dto);

            var expected = _dataContext.Commodities.FirstOrDefault();
            expected.Name.Should().Be(commodity.Name);
            expected.Code.Should().Be(commodity.Code);
            expected.Inventory.Should().Be(13);
        }

        [Fact]
        public void Update_update_BuyFactor_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            var buyFactor = BuyFactorFactory.GenerateBuyFactor(commodity.Code);
            _dataContext.Manipulate(_ => _.BuyFactors.Add(buyFactor));

            var dto = UpdateBuyFactorDtoFactory
                .GenerateUpdateBuyFactorDto(commodity.Code);

            _sut.Update(buyFactor.BuyFactorNumber, dto);

            var expected = _dataContext.BuyFactors.FirstOrDefault();
            expected.CommodityCode.Should().Be(dto.CommodityCode);
            expected.Date.Should().Be(dto.Date);
            expected.BuyPrice.Should().Be(dto.BuyPrice);
            expected.Count.Should().Be(dto.Count);
            expected.SellerName.Should().Be(dto.SellerName);
        }

        [Fact]
        public void Update_throw_BuyFactorNotFoundException_when_buyfactor_with_given_buyfactornumber_that_not_exist()
        {
            var fakebuyfactornumber = 170;

            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            var dto = UpdateBuyFactorDtoFactory
                .GenerateUpdateBuyFactorDto(commodity.Code);

            Action Expected = () => _sut.Update(fakebuyfactornumber, dto);
            Expected.Should().ThrowExactly<BuyFactorNotFoundException>();
        }

        [Fact]
        public void Delete_delete_BuyFactor_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            var buyFactor = BuyFactorFactory.GenerateBuyFactor(commodity.Code);
            _dataContext.Manipulate(_ => _.BuyFactors.Add(buyFactor));
            commodity.Inventory += buyFactor.Count;

            _sut.Delete(buyFactor.BuyFactorNumber);

            _dataContext.BuyFactors.Should().HaveCount(0);
        }

        [Fact]
        public void Delete_throw_BuyFactorNotFoundException_when_buyfactor_with_given_buyfactornumber_that_not_exist()
        {
            var fakebuyfactornumber = 20;

            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            Action Expected = () => _sut.Delete(fakebuyfactornumber);
            Expected.Should().ThrowExactly<BuyFactorNotFoundException>();
        }

        [Fact]
        public void Delete_Decrease_inventory_of_commodity_that_return_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            var buyFactor = BuyFactorFactory.GenerateBuyFactor(commodity.Code);
            _dataContext.Manipulate(_ => _.BuyFactors.Add(buyFactor));
            commodity.Inventory += buyFactor.Count;

            _sut.Delete(buyFactor.BuyFactorNumber);

            var expected = _dataContext.Commodities.FirstOrDefault();

            expected.Name.Should().Be(commodity.Name);
            expected.Code.Should().Be(commodity.Code);
            expected.Inventory.Should().Be(10);
        }

        [Fact]
        public void GetAll_return_all_BuyFactors_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            var buyFactor = BuyFactorFactory.GenerateBuyFactor(commodity.Code);
            _dataContext.Manipulate(_ => _.BuyFactors.Add(buyFactor));

            var secondbuyFactor = BuyFactorFactory
                .GenerateBuyFactor(commodity.Code);
            secondbuyFactor.Count = 1;
            secondbuyFactor.BuyPrice = "130000";
            _dataContext.Manipulate(_ => _.BuyFactors.Add(secondbuyFactor));

            commodity.Inventory += buyFactor.Count + secondbuyFactor.Count;

            var expected = _sut.GetAll();

            expected.Should().HaveCount(2);

            expected.Should().Contain(_ => _.CommodityCode==commodity.Code 
            && _.Count==buyFactor.Count && _.Date==buyFactor.Date 
            && _.BuyPrice==buyFactor.BuyPrice 
            && _.SellerName==buyFactor.SellerName);

            expected.Should().Contain(_ => _.CommodityCode == commodity.Code
            && _.Count == secondbuyFactor.Count && _.Date == secondbuyFactor.Date
            && _.BuyPrice == secondbuyFactor.BuyPrice 
            && _.SellerName == secondbuyFactor.SellerName);
        }
       
        [Fact]
        public void GetBuyFactor_return_buyfactor_with_Its_factornumber_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            var buyFactor = BuyFactorFactory.GenerateBuyFactor(commodity.Code);
            _dataContext.Manipulate(_ => _.BuyFactors.Add(buyFactor));
            commodity.Inventory += buyFactor.Count;

            var expected = _sut.GetBuyFactor(buyFactor.BuyFactorNumber);

            expected.CommodityCode.Should().Be(commodity.Code);
            expected.Date.Should().Be(buyFactor.Date);
            expected.Count.Should().Be(buyFactor.Count);
            expected.BuyPrice.Should().Be(buyFactor.BuyPrice);
            expected.SellerName.Should().Be(buyFactor.SellerName);
        }

        [Fact]
        public void GetBuyFactor_throw_BuyFactorNotFoundException_when_buyfactor_that_you_want_return_given_id_that_not_exist()
        {
            var fakebuyfactornumber = 7;

            Action Expected = () => _sut.GetBuyFactor(fakebuyfactornumber);
            Expected.Should().ThrowExactly<BuyFactorNotFoundException>();
        }
    }
}
