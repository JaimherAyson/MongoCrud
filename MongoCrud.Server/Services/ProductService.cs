using MongoCrud.Server.Models;
using MongoCrud.Server.Repositories;
using System;

namespace MongoCrud.Server.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    private readonly IProductRepository _repository = repository;

    public async Task<IEnumerable<Product>> GetAllAsync() =>
        await _repository.GetAllAsync();

    public async Task<Product> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Product ID cannot be empty.", nameof(id));

        return await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Product with ID '{id}' not found.");
    }

    public async Task CreateAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product, nameof(product));
        await _repository.CreateAsync(product);
    }

    public async Task UpdateAsync(string id, Product product)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Invalid product ID.", nameof(id));
        ArgumentNullException.ThrowIfNull(product, nameof(product));

        _ = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Product with ID '{id}' not found.");

        await _repository.UpdateAsync(id, product);
    }

    public async Task DeleteAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Invalid product ID.", nameof(id));

        _ = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Product with ID '{id}' not found.");

        await _repository.DeleteAsync(id);
    }
}
