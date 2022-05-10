using Microsoft.AspNetCore.Mvc;
using StoreManage.Services.SellFactors.Contracts;
using System.Collections.Generic;

namespace StoreManage.RestAPI.Controllers
{
    [Route("api/sellfactors")]
    [ApiController]
    public class SellFactorController : ControllerBase
    {
        private readonly SellFactorService _service;
        public SellFactorController(SellFactorService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddSellFactorDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet("{sellfactornumber}")]
        public GetSellFactorDto GetSellFactor(int sellfactornumber)
        {
            return _service.GetSellFactor(sellfactornumber);
        }

        [HttpGet]
        public IList<GetSellFactorDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpPut("{sellfactornumber}")]
        public void Update(int sellfactornumber, UpdateSellFactorDto dto)
        {
            _service.Update(sellfactornumber, dto);
        }

        [HttpDelete("{sellfactornumber}")]
        public void Delete(int sellfactornumber)
        {
            _service.Delete(sellfactornumber);
        }
    }
}
