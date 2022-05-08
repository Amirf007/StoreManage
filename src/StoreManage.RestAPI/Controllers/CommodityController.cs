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

        [HttpGet("{id}")]
        public GetCommodityDto GetCommodity(int id)
        {
            return _service.GetCommodity(id);
        }

        [HttpGet]
        public IList<GetCommodityDto> Getall()
        {
            return _service.GetAll();
        }

        [HttpPut("{id}")]
        public void Update(int id, UpdateCommodityDto dto)
        {
            _service.Update(id, dto);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _service.Delete(id);
        }
    }
}
