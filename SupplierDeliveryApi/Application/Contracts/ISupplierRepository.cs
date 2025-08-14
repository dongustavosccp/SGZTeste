using SupplierDeliveryAPI.Domain;

namespace SupplierDeliveryAPI.Application.Contracts
{
    public interface ISupplierRepository
    {
        Task<Supplier> GetSupplierById(int idSupplier);
        Task<List<Supplier>> GetAllSuppliers();
        Task<Supplier> AddAsync(Supplier supplier);
    }
}
