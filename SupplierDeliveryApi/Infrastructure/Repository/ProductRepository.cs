using Microsoft.EntityFrameworkCore;
using SupplierDeliveryAPI.Application.Contracts;
using SupplierDeliveryAPI.Domain;

namespace SupplierDeliveryAPI.Infrastructure.Repository
{

    public class ProductRepository : IProductRepository
    {
        private readonly SupplierDeliveryDbContext _context;

        public ProductRepository(SupplierDeliveryDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id) ?? new();
        }

        public async Task<Product> AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await GetProductById(id);
            if (product.IdProduct > 0)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.IdProduct == id);
        }
    }
}
