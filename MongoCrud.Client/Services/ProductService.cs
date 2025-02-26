using System.Net.Http.Json;
using MongoCrud.Client.Models;

namespace MongoCrud.Client.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Product>>("products") ?? new List<Product>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching products: {ex.Message}");
                return new List<Product>();
            }
        }

        public async Task<Product?> GetProductByIdAsync(string id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Product>($"products/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching product {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("products", product);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating product: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateProductAsync(string id, Product product)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"products/{id}", product);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"products/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product {id}: {ex.Message}");
                return false;
            }
        }
    }
}
