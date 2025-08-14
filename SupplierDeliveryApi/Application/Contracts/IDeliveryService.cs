using SupplierDeliveryAPI.Models;

namespace SupplierDeliveryAPI.Application.Contracts
{
    public interface IDeliveryService
    {
        Task<List<DeliveryResponse>> GetDeliveries(int supplierId, DateTime? startDate, DateTime? endDate);
        Task<DeliveryResponse> GetDeliveryById(int deliveryId);
        Task<DeliveryResponse> AddDelivery(DeliveryRequest delivery);
        Task<DeliveryResponse> UpdateDelivery(DeliveryRequest delivery);
        Task<List<DeliveryResponse>> GetDeliveriesBySupplier(int supplierId);
    }
}
