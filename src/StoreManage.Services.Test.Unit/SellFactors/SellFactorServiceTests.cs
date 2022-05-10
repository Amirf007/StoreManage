using FluentAssertions;
using StoreManage.Infrastructure.Application;
using StoreManage.Infrastructure.Test;
using StoreManage.Persistence.EF;
using StoreManage.Persistence.EF.Commodities;
using StoreManage.Persistence.EF.SellFactors;
using StoreManage.Services.Commodities.Contracts;
using StoreManage.Services.SellFactors;
using StoreManage.Services.SellFactors.Contracts;
using StoreManage.Test.Tools.Categories;
using StoreManage.Test.Tools.Commodities;
using StoreManage.Test.Tools.SellFactors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoreManage.Services.Test.Unit.SellFactors
{
    public class SellFactorServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly SellFactorService _sut;
        private readonly SellFactorRepository _repository;
        private readonly CommodityRepository _commodityRepository;

        public SellFactorServiceTests()
        {
            _dataContext =
                new EFInMemoryDataBase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFSellFactorRepository(_dataContext);
            _commodityRepository = new EFCommodityRepository(_dataContext);
            _sut = new SellFactorAppService(_repository, _unitOfWork, _commodityRepository);
        }

        [Fact]
        public void Add_adds_Sellfactor_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            var dto = AddSellFactorDtoFactory.GenerateAddSellFactorDto(commodity.Code);

            _sut.Add(dto);

            var expected = _dataContext.SellFactors.FirstOrDefault();
            expected.CommodityCode.Should().Be(commodity.Code);
            expected.Date.Should().Be(dto.Date);
            expected.BasePrice.Should().Be(dto.BasePrice);
            expected.TotalPrice.Should().Be(dto.TotalPrice);
            expected.Count.Should().Be(dto.Count);
            expected.BuyerName.Should().Be(dto.BuyerName);
        }

        [Fact]
        public void Add_decrease_inventory_of_commodity_that_exist_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));
            var initialbalance = commodity.Inventory;

            var dto = AddSellFactorDtoFactory.GenerateAddSellFactorDto(commodity.Code);

            _sut.Add(dto);

            var expected = _dataContext.Commodities.FirstOrDefault();
            expected.Name.Should().Be(commodity.Name);
            expected.Code.Should().Be(commodity.Code);
            expected.Inventory.Should().Be(initialbalance - dto.Count);
        }

        [Fact]
        public void Update_update_inventory_of_existcommodity_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));
            var initialbalance = commodity.Inventory;

            var sellFactor = SellFactorFactory.GenerateSellFactor(commodity.Code);
            _dataContext.Manipulate(_ => _.SellFactors.Add(sellFactor));
            commodity.Inventory -= sellFactor.Count;

            var dto = UpdateSellFactorDtoFactory.GenerateUpdateSellFactorDto(commodity.Code);

            _sut.Update(sellFactor.SellFactorNumber, dto);

            var expected = _dataContext.Commodities.FirstOrDefault();
            expected.Name.Should().Be(commodity.Name);
            expected.Code.Should().Be(commodity.Code);
            expected.Inventory.Should().Be(initialbalance - dto.Count);
        }

        [Fact]
        public void Update_update_sellfactor_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            var sellFactor = SellFactorFactory.GenerateSellFactor(commodity.Code);
            _dataContext.Manipulate(_ => _.SellFactors.Add(sellFactor));
            commodity.Inventory -= sellFactor.Count;

            var dto = UpdateSellFactorDtoFactory.GenerateUpdateSellFactorDto(commodity.Code);

            _sut.Update(sellFactor.SellFactorNumber, dto);

            var expected = _dataContext.SellFactors.FirstOrDefault();
            expected.CommodityCode.Should().Be(dto.CommodityCode);
            expected.Date.Should().Be(dto.Date);
            expected.BasePrice.Should().Be(dto.BasePrice);
            expected.TotalPrice.Should().Be(dto.TotalPrice);
            expected.Count.Should().Be(dto.Count);
            expected.BuyerName.Should().Be(dto.BuyerName);
        }

        [Fact]
        public void Update_throw_SellFactorNotFoundException_when_sellfactor_with_given_sellfactornumber_that_not_exist()
        {
            var fakesellfactornumber = 170;

            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            var dto = UpdateSellFactorDtoFactory.GenerateUpdateSellFactorDto(commodity.Code);

            Action Expected = () => _sut.Update(fakesellfactornumber, dto);
            Expected.Should().ThrowExactly<SellFactorNotFoundException>();
        }
    }
}
