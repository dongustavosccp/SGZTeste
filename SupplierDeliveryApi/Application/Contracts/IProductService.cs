using SupplierDeliveryAPI.Models;

namespace SupplierDeliveryAPI.Application.Contracts
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetProducts();
        Task<ProductDTO> GetProductById(int id);
        Task<ProductDTO> AddProduct(ProductDTO product);
        Task<ProductDTO> UpdateProduct(ProductDTO product);
        Task<bool> DeleteProduct(int id);
    }
}
