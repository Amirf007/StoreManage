using Microsoft.AspNetCore.Mvc;
using StoreManage.Services.BuyFactors.Contracts;
using System.Collections.Generic;

namespace StoreManage.RestAPI.Controllers
{
    [Route("api/buyfactors")]
    [ApiController]
    public class BuyFactorController : ControllerBase
    {
        private readonly BuyFactorService _service;
        public BuyFactorController(BuyFactorService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddBuyFactorDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet("{buyfactornumber}")]
        public GetBuyFactorDto GetBuyFactor(int buyfactornumber)
        {
            return _service.GetBuyFactor(buyfactornumber);
        }

        [HttpGet]
        public IList<GetBuyFactorDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpPut("{buyfactornumber}")]
        public void Update(int buyfactornumber, UpdateBuyFactorDto dto)
        {
            _service.Update(buyfactornumber, dto);
        }

        [HttpDelete("{buyfactornumber}")]
        public void Delete(int buyfactornumber)
        {
            _service.Delete(buyfactornumber);
        }
    }
}
