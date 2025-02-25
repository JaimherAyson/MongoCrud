using MongoCrud.Server.Models;
using MongoCrud.Server.Repositories;

namespace MongoCrud.Server.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Product> GetByIdAsync(string id) => await _repository.GetByIdAsync(id);
        public async Task CreateAsync(Product product) => await _repository.CreateAsync(product);
        public async Task UpdateAsync(string id, Product product) => await _repository.UpdateAsync(id, product);
        public async Task DeleteAsync(string id) => await _repository.DeleteAsync(id);
    }
}