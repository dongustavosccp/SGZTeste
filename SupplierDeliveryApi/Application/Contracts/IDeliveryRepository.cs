using SupplierDeliveryAPI.Domain;

namespace SupplierDeliveryAPI.Application.Contracts
{
    public interface IDeliveryRepository
    {
        Task<List<Delivery>> GetDeliveries(int supplierId, DateTime? startDate, DateTime? endDate);
        Task<List<Delivery>> GetDeliveriesBySupplier(int supplierId);
        Task<Delivery> GetDeliveryById(int deliveryId);
        Task<Delivery> AddDelivery(Delivery delivery);
        Task<Delivery> UpdateDelivery(Delivery delivery);
    }
}