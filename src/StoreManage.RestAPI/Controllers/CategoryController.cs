using Microsoft.AspNetCore.Mvc;
using StoreManage.Services.Categories.Contracts;
using System.Collections.Generic;

namespace StoreManage.RestAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _service;
        public CategoryController(CategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddCategoryDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet("{id}")]
        public GetCategoryDto GetCategory(int id)
        {
            return _service.GetCategory(id);
        }

        [HttpGet]
        public IList<GetCategoryDto> Getall()
        {
            return _service.GetAll();
        }

        [HttpPut("{id}")]
        public void Update(int id, UpdateCategoryDto dto)
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
