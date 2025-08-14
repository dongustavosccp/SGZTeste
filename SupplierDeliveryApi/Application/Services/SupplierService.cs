using SupplierDeliveryAPI.Application.Contracts;
using SupplierDeliveryAPI.Domain;
using SupplierDeliveryAPI.Models;
using Utils;
using Utils.DomainNotification;

namespace SupplierDeliveryAPI.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repository;
        private readonly INotifier _notifier;

        public SupplierService(ISupplierRepository repository, INotifier notifier)
        {
            _repository = repository;
            _notifier = notifier;
        }

        public async Task<SupplierDTO> AddAsync(SupplierDTO supplier)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(supplier.NmSupplier))
                {
                    _notifier.AddNotification(new ErrorMessage("Supplier name cannot be empty."));
                    return new();
                }

                Supplier supplierResponse = await _repository.AddAsync(new()
                {
                    NmSupplier = supplier.NmSupplier
                });

                return new()
                {
                    IdSupplier = supplierResponse.IdSupplier,
                    NmSupplier = supplierResponse.NmSupplier
                };
            }
            catch (Exception ex)
            {
                _notifier.AddNotification(new ErrorMessage(ex.Message));
                return new();
            }
        }

        public async Task<List<SupplierDTO>> GetAllSuppliers()
        {
            try
            {
                List<Supplier> suppliers = await _repository.GetAllSuppliers();
                return suppliers.Select(s => new SupplierDTO
                {
                    IdSupplier = s.IdSupplier,
                    NmSupplier = s.NmSupplier,
                }).ToList() ?? new();
            }
            catch (Exception ex)
            {
                _notifier.AddNotification(new ErrorMessage(ex.Message));
                return new();
            }
        }

        public async Task<SupplierDTO> GetSupplierById(int idSupplier)
        {
            try
            {
                Supplier supplier = await _repository.GetSupplierById(idSupplier);
                if (supplier.IdSupplier <= 0)
                {
                    _notifier.AddNotification(new ErrorMessage("Supplier not found."));
                    return new();
                }

                return new()
                {
                    IdSupplier = supplier.IdSupplier,
                    NmSupplier = supplier.NmSupplier,
                };
            }
            catch (Exception ex)
            {
                _notifier.AddNotification(new ErrorMessage(ex.Message));
                return new();
            }
        }
    }
}
