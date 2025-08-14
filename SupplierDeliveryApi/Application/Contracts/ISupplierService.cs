using SupplierDeliveryAPI.Domain;
using SupplierDeliveryAPI.Models;
using Utils;

namespace SupplierDeliveryAPI.Application.Contracts
{
    public interface ISupplierService
    {
        Task<SupplierDTO> GetSupplierById(int idSupplier);
        Task<List<SupplierDTO>> GetAllSuppliers();
        Task<SupplierDTO> AddAsync(SupplierDTO supplier);
    }
}
