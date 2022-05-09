using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StoreManage.Entities;
using StoreManage.Infrastructure.Application;
using StoreManage.Infrastructure.Test;
using StoreManage.Persistence.EF;
using StoreManage.Persistence.EF.Categories;
using StoreManage.Persistence.EF.Commodities;
using StoreManage.Services.Categories.Contracts;
using StoreManage.Services.Commodities;
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

namespace StoreManage.Services.Test.Unit.Commodities
{
    public class CommodityServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly CommodityService _sut;
        private readonly CommodityRepository _repository;
        private readonly CategoryRepository _categoryRepository;

        public CommodityServiceTests()
        {
            _dataContext =
                new EFInMemoryDataBase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCommodityRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CommodityAppService(_repository, _unitOfWork,_categoryRepository);
        }

        [Fact]
        public void Add_adds_commodity_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            AddCommodityDto dto = AddCommodityDtoFactory.GenerateAddCommodityDto(category.Id);

            _sut.Add(dto);

            var expected = _dataContext.Commodities
                .Include(_ => _.Category)
                .FirstOrDefault();
            expected.Name.Should().Be(dto.Name);
            expected.Price.Should().Be(dto.Price);
            expected.Inventory.Should().Be(dto.Inventory);
            expected.MaxInventory.Should().Be(dto.MaxInventory);
            expected.MinInventory.Should().Be(dto.MinInventory);
            expected.Category.Id.Should().Be(category.Id);
        }

        [Fact]
        public void Update_update_commodity_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            UpdateCommodityDto dto = UpdateCommodityDtoFactory.GenerateUpdateCommodityDto(category.Id);
            dto.Name = "ماست رامک";
            dto.Price = "170000";

            _sut.Update(category.Id, dto);

            var Expected = _dataContext.Commodities
                .FirstOrDefault();
            Expected.Name.Should().Be(dto.Name);
            Expected.Price.Should().Be(dto.Price);
            Expected.Inventory.Should().Be(dto.Inventory);
            Expected.MaxInventory.Should().Be(dto.MaxInventory);
            Expected.MinInventory.Should().Be(dto.MinInventory);
            Expected.Category.Id.Should().Be(dto.CategoryId);
        }

        [Fact]
        public void Update_throw_DuplicateCommodityNameInCategoryException_When_commodity_update_with_duplicate_name_and_categoryId_with_different_id()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            var existcommodity = CommodityFactory.CreateCommodity(category.Id);
            existcommodity.Code = 2;
            existcommodity.Name = "شیر پر چرب رامک";
            existcommodity.Price = "170000";
            existcommodity.Inventory = 9;
            _dataContext.Manipulate(_ => _.Commodities.Add(existcommodity));

            UpdateCommodityDto dto = UpdateCommodityDtoFactory.GenerateUpdateCommodityDto(category.Id);
            dto.Name = existcommodity.Name;

            Action Expected = () => _sut.Update(commodity.Code, dto);
            Expected.Should()
                .ThrowExactly<DuplicateCommodityNameInCategoryException>();
        }

        [Fact]
        public void Update_throw_CommodityNotFoundException_when_commodity_with_given_id_that_not_exist()
        {
            var fakecommodityCode = 100;

            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            UpdateCommodityDto dto = UpdateCommodityDtoFactory.GenerateUpdateCommodityDto(category.Id);

            Action Expected = () => _sut.Update(fakecommodityCode, dto);
            Expected.Should().ThrowExactly<CommodityNotFoundException>();
        }

        [Fact]
        public void Delete_delete_commodity_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            _sut.Delete(commodity.Code);

            _dataContext.Commodities.Should().HaveCount(0);
        }

        [Fact]
        public void Delete_throw_CommodityNotFoundException_when_commodity_that_want_to_be_delete_given_id_that_not_exist()
        {
            var fakecommodityId = 100;

            Action Expected = () => _sut.Delete(fakecommodityId);
            Expected.Should().ThrowExactly<CommodityNotFoundException>();
        }

        [Fact]
        public void Getall_return_all_commodities_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodities = CommoditiesFactory.GenerateCommodities(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.AddRange(commodities));

            var expected = _sut.GetAll();

            expected.Should().HaveCount(2);
            expected.Should().Contain(_ => _.Name == "شیر رامک" && _.Price == "150000" && _.Inventory == 10 && _.MaxInventory == "15" && _.MinInventory == "5" && _.CategoryId == category.Id);
            expected.Should().Contain(_ => _.Name == "دوغ رامک" && _.Price == "120000" && _.Inventory == 15 && _.MaxInventory == "20" && _.MinInventory == "7" && _.CategoryId == category.Id);
        }

        [Fact]
        public void GetCommodity_return_commodity_with_Id()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            var expected = _sut.GetCommodity(commodity.Code);

            expected.Name.Should().Be(commodity.Name);
            expected.Price.Should().Be(commodity.Price);
            expected.Inventory.Should().Be(commodity.Inventory);
            expected.MaxInventory.Should().Be(commodity.MaxInventory);
            expected.MinInventory.Should().Be(commodity.MinInventory);
            expected.CategoryId.Should().Be(category.Id);
        }

        [Fact]
        public void GetCommodity_throw_CommodityNotFoundException_when_commodity_that_you_want_return_given_id_that_not_exist()
        {
            var fakecommodityId = 102;

            Action Expected = () => _sut.GetCommodity(fakecommodityId);
            Expected.Should().ThrowExactly<CommodityNotFoundException>();
        }
    }
}
