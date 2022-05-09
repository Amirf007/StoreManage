using Microsoft.AspNetCore.Mvc;
using StoreManage.Services.Commodities.Contracts;
using System.Collections.Generic;

namespace StoreManage.RestAPI.Controllers
{
    [Route("api/commodities")]
    [ApiController]
    public class CommodityController : ControllerBase
    {
        private readonly CommodityService _service;
        public CommodityController(CommodityService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddCommodityDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet("{code}")]
        public GetCommodityDto GetCommodity(int code)
        {
            return _service.GetCommodity(code);
        }

        [HttpGet]
        public IList<GetCommodityDto> Getall()
        {
            return _service.GetAll();
        }

        [HttpPut("{code}")]
        public void Update(int code, UpdateCommodityDto dto)
        {
            _service.Update(code, dto);
        }

        [HttpDelete("{code}")]
        public void Delete(int code)
        {
            _service.Delete(code);
        }
    }
}
