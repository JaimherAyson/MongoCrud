using MongoCrud.Server.Data;
using MongoCrud.Server.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using System;

namespace MongoCrud.Server.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _products;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(IMongoDbContext context, ILogger<ProductRepository> logger)
    {
        _products = context.GetCollection<Product>("Products");
        _logger = logger;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        try
        {
            return await _products.Find(_ => true).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve all products.");
            throw new ApplicationException("Failed to retrieve all products.", ex);
        }
    }

    public async Task<Product> GetByIdAsync(string id)
    {
        try
        {
            var product = await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
            return product ?? throw new KeyNotFoundException($"Product with ID '{id}' not found.");
        }
        catch (Exception ex) when (ex is not KeyNotFoundException)
        {
            _logger.LogError(ex, "Failed to retrieve product with ID: {ProductId}", id);
            throw new ApplicationException($"Failed to retrieve product with ID: {id}", ex);
        }
    }

    public async Task CreateAsync(Product product)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(product);
            await _products.InsertOneAsync(product);
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "Database error while creating a product.");
            throw new ApplicationException("Database error while creating a product.", ex);
        }
    }

    public async Task UpdateAsync(string id, Product product)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Invalid product ID.", nameof(id));
            ArgumentNullException.ThrowIfNull(product);

            var result = await _products.ReplaceOneAsync(p => p.Id == id, product);
            if (result.MatchedCount == 0)
                throw new KeyNotFoundException($"Product with ID '{id}' not found.");
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "Database error while updating product with ID: {ProductId}", id);
            throw new ApplicationException($"Database error while updating product with ID: {id}", ex);
        }
    }

    public async Task DeleteAsync(string id)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Invalid product ID.", nameof(id));

            var result = await _products.DeleteOneAsync(p => p.Id == id);
            if (result.DeletedCount == 0)
                throw new KeyNotFoundException($"Product with ID '{id}' not found.");
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "Database error while deleting product with ID: {ProductId}", id);
            throw new ApplicationException($"Database error while deleting product with ID: {id}", ex);
        }
    }
}
