using MongoCrud.Server.Data;
using MongoCrud.Server.Models;
using MongoDB.Driver;

namespace MongoCrud.Server.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(IMongoDbContext context)
        {
            _products = context.GetCollection<Product>("Products");
        }

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await _products.Find(p => true).ToListAsync();

        public async Task<Product> GetByIdAsync(string id) =>
            await _products.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Product product) =>
            await _products.InsertOneAsync(product);

        public async Task UpdateAsync(string id, Product product) =>
            await _products.ReplaceOneAsync(p => p.Id == id, product);

        public async Task DeleteAsync(string id) =>
            await _products.DeleteOneAsync(p => p.Id == id);
    }
}