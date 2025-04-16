using CatalogApi.CoreServices.Services.Interfaces;
using CatalogApi.Domain.Models.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogApi.Service.Controllers
{
    [Route("api/category/[Action]")]
    [Authorize]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        
        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _categoryService.GetById(id);
            return StatusCode((int)response.StatusCode, response);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryModel request)
        {
            var response = await _categoryService.CreateAsync(request);
            return StatusCode((int)response.StatusCode, response.Message);

        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
