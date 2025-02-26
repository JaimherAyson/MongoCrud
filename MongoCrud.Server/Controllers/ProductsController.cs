using Microsoft.AspNetCore.Mvc;
using MongoCrud.Server.DTOs;
using MongoCrud.Server.Models;
using MongoCrud.Server.Services;

namespace MongoCrud.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService service, ILogger<ProductsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var products = await _service.GetAllAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching products.");
            return StatusCode(500, new { message = "An error occurred while fetching products." });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var product = await _service.GetByIdAsync(id);
            return product is not null
                ? Ok(product)
                : NotFound(new { message = $"Product with ID {id} not found." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching product with ID {ProductId}.", id);
            return StatusCode(500, new { message = "An error occurred while fetching the product." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto dto)
    {
        try
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Invalid product data." });

            var product = new Product { Name = dto.Name, Price = dto.Price };
            await _service.CreateAsync(product);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product.");
            return StatusCode(500, new { message = "An error occurred while creating the product." });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] ProductDto dto)
    {
        try
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Invalid product data." });

            var product = new Product { Id = id, Name = dto.Name, Price = dto.Price };
            await _service.UpdateAsync(id, product);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Product with ID {id} not found." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product with ID {ProductId}.", id);
            return StatusCode(500, new { message = "An error occurred while updating the product." });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Product with ID {id} not found." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with ID {ProductId}.", id);
            return StatusCode(500, new { message = "An error occurred while deleting the product." });
        }
    }
}
