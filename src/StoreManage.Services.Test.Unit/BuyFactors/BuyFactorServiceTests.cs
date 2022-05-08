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
            _sut = new BuyFactorAppService(_repository, _unitOfWork, _commodityRepository);
        }

        [Fact]
        public void Add_adds_entrycommodity_properly()
        {
            var category = CategoryFactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var commodity = CommodityFactory.CreateCommodity(category.Id);
            _dataContext.Manipulate(_ => _.Commodities.Add(commodity));

            AddBuyFactorDto dto = AddBuyFactorDtoFactory.GenerateAddBuyFactorDto(commodity.Code);

            _sut.Add(dto);

            var expected = _dataContext.Commodities.FirstOrDefault();
            expected.Name.Should().Be(commodity.Name);
            expected.Code.Should().Be(dto.CommodityCode);
            expected.Inventory.Should().Be(commodity.Inventory);
        }
    }
}
