using Microsoft.EntityFrameworkCore;
using SupplierDeliveryAPI.Application.Contracts;
using SupplierDeliveryAPI.Domain;

namespace SupplierDeliveryAPI.Infrastructure.Repository
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly SupplierDeliveryDbContext _context;
        public DeliveryRepository(SupplierDeliveryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Delivery>> GetDeliveries(int supplierId, DateTime? startDate, DateTime? endDate)
        {
            IQueryable<Delivery> query = _context.Deliveries.AsQueryable();
            if (supplierId > 0)
                query = query.Where(d => d.IdSupplier == supplierId);
            if (startDate.HasValue)
                query = query.Where(d => d.DtDelivery >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(d => d.DtDelivery <= endDate.Value);
            return await query.Include(d => d.Supplier)
                .Include(d => d.Products)
                    .ThenInclude(dp => dp.Product)
                .Include(d => d.Address)
                .ToListAsync() ?? new();
        }

        public async Task<List<Delivery>> GetDeliveriesBySupplier(int supplierId)
        {
            return await _context.Deliveries
                .Where(d => d.IdSupplier == supplierId)
                .Include(d => d.Supplier)
                .Include(d => d.Products)
                    .ThenInclude(dp => dp.Product)
                .Include(d => d.Address)
                .ToListAsync() ?? new();
        }

        public async Task<Delivery> GetDeliveryById(int deliveryId)
        {
            return await _context.Deliveries
                .Include(d => d.Supplier)
                .Include(d => d.Products)
                    .ThenInclude(dp => dp.Product)
                .Include(d => d.Address)
                .FirstOrDefaultAsync(d => d.IdDelivery == deliveryId) ?? new();
        }

        public async Task<Delivery> AddDelivery(Delivery delivery)
        {
            await _context.Deliveries.AddAsync(delivery);
            await _context.SaveChangesAsync();
            return delivery;
        }

        public async Task<Delivery> UpdateDelivery(Delivery delivery)
        {
            _context.Deliveries.Update(delivery);
            await _context.SaveChangesAsync();
            return delivery;
        }
    }
}
