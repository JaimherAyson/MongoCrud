using Microsoft.AspNetCore.Mvc;
using MongoCrud.Server.DTOs;
using MongoCrud.Server.Models;
using MongoCrud.Server.Services;

namespace MongoCrud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id) => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto dto)
        {
            var product = new Product { Name = dto.Name, Price = dto.Price };
            await _service.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ProductDto dto)
        {
            var product = new Product { Id = id, Name = dto.Name, Price = dto.Price };
            await _service.UpdateAsync(id, product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}