using SupplierDeliveryAPI.Application.Contracts;
using SupplierDeliveryAPI.Domain;
using SupplierDeliveryAPI.Models;
using Utils;
using Utils.DomainNotification;

namespace SupplierDeliveryAPI.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly INotifier _notifier;

        public ProductService(IProductRepository productRepository, INotifier notifier)
        {
            _productRepository = productRepository;
            _notifier = notifier;
        }

        public async Task<ProductDTO> AddProduct(ProductDTO product)
        {
            try
            {
                Product productEntity = new();
                Product productResponse = new();
                if (string.IsNullOrWhiteSpace(product.NmProduct))
                {
                    _notifier.AddNotification(new ErrorMessage("Product name cannot be empty."));
                    return new();
                }

                if (product.VlProduct <= 0)
                {
                    _notifier.AddNotification(new ErrorMessage("Product price must be greater than zero."));
                    return new();
                }

                productEntity = await _productRepository.GetProductById(product.IdProduct);

                if (productEntity.IdProduct > 0)
                {
                    productResponse = await _productRepository.UpdateProduct(productEntity);
                    return new()
                    {
                        IdProduct = productResponse.IdProduct,
                        NmProduct = productResponse.NmProduct,
                        DsProduct = productResponse.Description,
                        VlProduct = productResponse.Price
                    };
                }
                productResponse = await _productRepository.AddProduct(productEntity);
                return new()
                {
                    IdProduct = productResponse.IdProduct,
                    NmProduct = productResponse.NmProduct,
                    DsProduct = productResponse.Description,
                    VlProduct = productResponse.Price
                };
            }
            catch (Exception ex)
            {
                _notifier.AddNotification(new ErrorMessage(ex.Message));
                return new();
            }
        }

        public async Task<ProductDTO> UpdateProduct(ProductDTO product)
        {
            try
            {
                Product productEntity = new()
                {
                    IdProduct = product.IdProduct,
                    NmProduct = product.NmProduct,
                    Description = product.DsProduct,
                    Price = product.VlProduct
                };

                if (!await _productRepository.ProductExistsAsync(productEntity.IdProduct))
                {
                    _notifier.AddNotification(new ErrorMessage("Product not found."));
                    return new();
                }

                Product updatedProduct = await _productRepository.UpdateProduct(productEntity);
                return new()
                {
                    IdProduct = updatedProduct.IdProduct,
                    NmProduct = updatedProduct.NmProduct,
                    DsProduct = updatedProduct.Description,
                    VlProduct = updatedProduct.Price
                };
            }
            catch (Exception ex)
            {
                _notifier.AddNotification(new ErrorMessage(ex.Message));
                return new();
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                if (!await _productRepository.ProductExistsAsync(id))
                {
                    _notifier.AddNotification(new ErrorMessage("Product not found."));
                    return false;
                }

                bool deleted = await _productRepository.DeleteProduct(id);

                if (!deleted)
                {
                    _notifier.AddNotification(new ErrorMessage("Failed to delete product."));
                }

                return deleted;
            }
            catch (Exception ex)
            {
                _notifier.AddNotification(new ErrorMessage(ex.Message));
                return false;
            }
        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            try
            {
                Product product = await _productRepository.GetProductById(id);

                if (product.IdProduct <= 0)
                {
                    _notifier.AddNotification(new ErrorMessage("Product not found."));
                    return new();
                }

                return new()
                {
                    IdProduct = product.IdProduct,
                    NmProduct = product.NmProduct,
                    DsProduct = product.Description,
                    VlProduct = product.Price
                };
            }
            catch (Exception ex)
            {
                _notifier.AddNotification(new ErrorMessage(ex.Message));
                return new();
            }
        }

        public async Task<List<ProductDTO>> GetProducts()
        {
            try
            {
                IEnumerable<Product> response = await _productRepository.GetProducts();

                if (!response.Any())
                {
                    _notifier.AddNotification(new ErrorMessage("No products found."));
                    return new List<ProductDTO>();
                }

                return response.Select(p => new ProductDTO
                {
                    IdProduct = p.IdProduct,
                    NmProduct = p.NmProduct,
                    DsProduct = p.Description,
                    VlProduct = p.Price
                }).ToList() ?? new();
            }
            catch (Exception ex)
            {
                _notifier.AddNotification(new ErrorMessage(ex.Message));
                return new();
            }
        }
    }
}
