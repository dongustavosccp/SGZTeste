using SupplierDeliveryAPI.Domain;

namespace SupplierDeliveryAPI.Application.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProductById(int id);
        Task<Product> AddProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int id);
        Task<bool> ProductExistsAsync(int id);
    }
}
