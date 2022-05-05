using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StoreManage.Infrastructure.Application;
using StoreManage.Infrastructure.Test;
using StoreManage.Persistence.EF;
using StoreManage.Persistence.EF.Categories;
using StoreManage.Persistence.EF.Commodities;
using StoreManage.Services.Categories.Contracts;
using StoreManage.Services.Commodities;
using StoreManage.Services.Commodities.Contracts;
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

            AddCommodityDto dto = GenerateAddCommodityDto(category);

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
        public void Add_throws_DuplicateCommodityNameInCategoryException_when_commodity_register_with_duplicate_Name()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            var dto = new AddCommodityDto
            {
                Name = commodity.Name,
                Price = "200000",
                Inventory = 10,
                MaxInventory = "15",
                MinInventory = "5",
                CategoryId = category.Id,
            };

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<DuplicateCommodityNameInCategoryException>();
        }

        [Fact]
        public void Update_update_commodity_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            UpdateCommodityDto dto = new UpdateCommodityDto
            {
                Name = "ماست رامک",
                Price = "170000",
                Inventory = 10,
                MaxInventory = "15",
                MinInventory = "5",
                CategoryId = category.Id,
            };

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
            existcommodity.Name = "شیر پر چرب رامک";
            existcommodity.Price = "170000";
            existcommodity.Inventory = 9;
            _dataContext.Manipulate(_ => _.Commodities.Add(existcommodity));

            UpdateCommodityDto dto = GenerateUpdateCommodityDto(category, commodity);

            Action Expected = () => _sut.Update(commodity.Code, dto);
            Expected.Should()
                .ThrowExactly<DuplicateCommodityNameInCategoryException>();
        }

        private static UpdateCommodityDto GenerateUpdateCommodityDto(Entities.Category category, Entities.Commodity commodity)
        {
            return new UpdateCommodityDto
            {
                Name = "شیر پر چرب رامک",
                Price = commodity.Price,
                Inventory = commodity.Inventory,
                MaxInventory = commodity.MaxInventory,
                MinInventory = commodity.MinInventory,
                CategoryId = category.Id,
            };
        }

        [Fact]
        public void Update_throw_CommodityNotFoundException_when_commodity_with_given_id_that_not_exist()
        {
            var fakecommodityCode = 100;

            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            UpdateCommodityDto dto = GenerateUpdateCommodityDto(category,commodity);

            Action Expected = () => _sut.Update(fakecommodityCode, dto);
            Expected.Should().ThrowExactly<CommodityNotFoundException>();
        }

        private static AddCommodityDto GenerateAddCommodityDto(Entities.Category category)
        {
            return new AddCommodityDto
            {
                Name = "شیر رامک",
                Price = "150000",
                Inventory = 10,
                MaxInventory = "15",
                MinInventory = "5",
                CategoryId = category.Id,
            };
        }
    }
}
