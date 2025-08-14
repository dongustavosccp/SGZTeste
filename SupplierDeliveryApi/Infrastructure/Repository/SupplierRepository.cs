using Microsoft.EntityFrameworkCore;
using SupplierDeliveryAPI.Application.Contracts;
using SupplierDeliveryAPI.Domain;

namespace SupplierDeliveryAPI.Infrastructure.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly SupplierDeliveryDbContext _context;

        public SupplierRepository(SupplierDeliveryDbContext context)
        {
            _context = context;
        }

        public async Task<Supplier> AddAsync(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<List<Supplier>> GetAllSuppliers()
        {
            return await _context.Suppliers.ToListAsync() ?? new();
        }

        public async Task<Supplier> GetSupplierById(int idSupplier)
        {
            return await _context.Suppliers.FindAsync(idSupplier) ?? new();
        }
    }
}
