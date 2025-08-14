using SupplierDeliveryAPI.Application.Contracts;
using SupplierDeliveryAPI.Domain;
using SupplierDeliveryAPI.Models;
using Utils;
using Utils.DomainNotification;

namespace SupplierDeliveryAPI.Application.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _repository;
        private readonly INotifier _notifier;
        public DeliveryService(IDeliveryRepository repository, INotifier notifier)
        {
            _repository = repository;
            _notifier = notifier;
        }

        public async Task<DeliveryResponse> AddDelivery(DeliveryRequest delivery)
        {
            try
            {
                if (delivery.IdSupplier <= 0)
                {
                    _notifier.AddNotification(new ErrorMessage("Supplier ID must be greater than zero."));
                    return new();
                }

                if (delivery.Products == null || !delivery.Products.Any())
                {
                    _notifier.AddNotification(new ErrorMessage("At least one product must be included in the delivery."));
                    return new();
                }

                Delivery domainDelivery = new()
                {
                    DtDelivery = delivery.DtDelivery,
                    DsStatus = delivery.DsStatus,
                    IdSupplier = delivery.IdSupplier,
                    Address = new()
                    {
                        Street = delivery.Address.Street,
                        City = delivery.Address.City,
                        Number = delivery.Address.Number,
                        ZipCode = delivery.Address.ZipCode,
                        State = delivery.Address.State
                    },
                    Products = delivery.Products.Select(p => new DeliveryProduct
                    {
                        IdProduct = p.IdProduct,
                        QtProduct = p.QtProduct
                    }).ToList()
                };

                Delivery savedDelivery = await _repository.AddDelivery(domainDelivery);

                if (savedDelivery == null || savedDelivery.IdDelivery <= 0)
                {
                    _notifier.AddNotification(new ErrorMessage("Failed to save delivery."));
                    return new();
                }

                return new()
                {
                    IdDelivery = savedDelivery.IdDelivery,
                    DtDelivery = savedDelivery.DtDelivery,
                    DsStatus = savedDelivery.DsStatus,
                    Supplier = new()
                    {
                        IdSupplier = savedDelivery.Supplier.IdSupplier,
                        NmSupplier = savedDelivery.Supplier.NmSupplier
                    },
                    Address = new()
                    {
                        Street = savedDelivery.Address.Street,
                        Number = savedDelivery.Address.Number,
                        City = savedDelivery.Address.City,
                        State = savedDelivery.Address.State,
                        ZipCode = savedDelivery.Address.ZipCode
                    },
                    Products = savedDelivery.Products.Select(p => new DeliveryProductResponse
                    {
                        IdProduct = p.Product.IdProduct,
                        NmProduct = p.Product.NmProduct,
                        QtProduct = p.QtProduct
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _notifier.AddNotification(new ErrorMessage(ex.Message));
                return new();
            }
        }

        public async Task<List<DeliveryResponse>> GetDeliveries(int supplierId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                IEnumerable<Domain.Delivery> deliveries = await _repository.GetDeliveries(supplierId, startDate, endDate);
                return deliveries.Select(d => new DeliveryResponse
                {
                    IdDelivery = d.IdDelivery,
                    DtDelivery = d.DtDelivery,
                    DsStatus = d.DsStatus,
                    Supplier = new()
                    {
                        IdSupplier = d.Supplier.IdSupplier,
                        NmSupplier = d.Supplier.NmSupplier
                    },
                    Address = new()
                    {
                        Street = d.Address.Street,
                        Number = d.Address.Number,
                        City = d.Address.City,
                        State = d.Address.State,
                        ZipCode = d.Address.ZipCode
                    },
                    Products = d.Products.Select(p => new DeliveryProductResponse
                    {
                        IdProduct = p.Product.IdProduct,
                        NmProduct = p.Product.NmProduct,
                        QtProduct = p.QtProduct
                    }).ToList()
                }).ToList() ?? new();
            }
            catch (Exception ex)
            {
                _notifier.AddNotification(new ErrorMessage(ex.Message));
                return new();
            }
        }

        public async Task<List<DeliveryResponse>> GetDeliveriesBySupplier(int supplierId)
        {
            try
            {
                if (supplierId <= 0)
                {
                    _notifier.AddNotification(new ErrorMessage("Supplier ID must be greater than zero."));
                    return new();
                }

                List<Delivery> deliveries = await _repository.GetDeliveriesBySupplier(supplierId);
                return deliveries.Select(d => new DeliveryResponse
                {
                    IdDelivery = d.IdDelivery,
                    DtDelivery = d.DtDelivery,
                    DsStatus = d.DsStatus,
                    Supplier = new()
                    {
                        IdSupplier = d.Supplier.IdSupplier,
                        NmSupplier = d.Supplier.NmSupplier
                    },
                    Address = new()
                    {
                        Street = d.Address.Street,
                        Number = d.Address.Number,
                        City = d.Address.City,
                        State = d.Address.State,
                        ZipCode = d.Address.ZipCode
                    },
                    Products = d.Products.Select(p => new DeliveryProductResponse
                    {
                        IdProduct = p.Product.IdProduct,
                        NmProduct = p.Product.NmProduct,
                        QtProduct = p.QtProduct
                    }).ToList()
                }).ToList() ?? new();
            }
            catch (Exception ex)
            {
                _notifier.AddNotification(new ErrorMessage(ex.Message));
                return new();
            }
        }

        public async Task<DeliveryResponse> GetDeliveryById(int deliveryId)
        {
            try
            {
                if (deliveryId <= 0)
                {
                    _notifier.AddNotification(new ErrorMessage("Delivery ID must be greater than zero."));
                    return new();
                }

                Domain.Delivery delivery = await _repository.GetDeliveryById(deliveryId);
                if (delivery.IdDelivery <= 0)
                {
                    _notifier.AddNotification(new ErrorMessage("Delivery not found."));
                    return new();
                }

                return new()
                {
                    IdDelivery = delivery.IdDelivery,
                    DtDelivery = delivery.DtDelivery,
                    DsStatus = delivery.DsStatus,
                    Supplier = new()
                    {
                        IdSupplier = delivery.Supplier.IdSupplier,
                        NmSupplier = delivery.Supplier.NmSupplier
                    },
                    Address = new()
                    {
                        Street = delivery.Address.Street,
                        Number = delivery.Address.Number,
                        City = delivery.Address.City,
                        State = delivery.Address.State,
                        ZipCode = delivery.Address.ZipCode
                    },
                    Products = delivery.Products.Select(p => new DeliveryProductResponse
                    {
                        IdProduct = p.Product.IdProduct,
                        NmProduct = p.Product.NmProduct,
                        QtProduct = p.QtProduct
                    }).ToList()
                };

            }
            catch (Exception ex)
            {

                return new();
            }
        }

        public async Task<DeliveryResponse> UpdateDelivery(DeliveryRequest delivery)
        {
            try
            {
                if (delivery.IdSupplier <= 0)
                {
                    _notifier.AddNotification(new ErrorMessage("Supplier ID must be greater than zero."));
                    return new();
                }
                if (delivery.Products == null || !delivery.Products.Any())
                {
                    _notifier.AddNotification(new ErrorMessage("At least one product must be included in the delivery."));
                    return new();
                }
                Delivery domainDelivery = new()
                {
                    IdDelivery = delivery.IdDelivery,
                    DtDelivery = delivery.DtDelivery,
                    DsStatus = delivery.DsStatus,
                    IdSupplier = delivery.IdSupplier,
                    Address = new()
                    {
                        Street = delivery.Address.Street,
                        City = delivery.Address.City,
                        Number = delivery.Address.Number,
                        ZipCode = delivery.Address.ZipCode,
                        State = delivery.Address.State
                    },
                    Products = delivery.Products.Select(p => new DeliveryProduct
                    {
                        IdProduct = p.IdProduct,
                        QtProduct = p.QtProduct
                    }).ToList()
                };
                Delivery updatedDelivery = await _repository.UpdateDelivery(domainDelivery);
                if (updatedDelivery == null || updatedDelivery.IdDelivery <= 0)
                {
                    _notifier.AddNotification(new ErrorMessage("Failed to update delivery."));
                    return new();
                }
                return new()
                {
                    IdDelivery = updatedDelivery.IdDelivery,
                    DtDelivery = updatedDelivery.DtDelivery,
                    DsStatus = updatedDelivery.DsStatus,
                    Supplier = new()
                    {
                        IdSupplier = updatedDelivery.Supplier.IdSupplier,
                        NmSupplier = updatedDelivery.Supplier.NmSupplier
                    },
                    Address = new()
                    {
                        Street = updatedDelivery.Address.Street,
                        Number = updatedDelivery.Address.Number,
                        City = updatedDelivery.Address.City,
                        State = updatedDelivery.Address.State,
                        ZipCode = updatedDelivery.Address.ZipCode
                    },
                    Products = updatedDelivery.Products.Select(p => new DeliveryProductResponse
                    {
                        IdProduct = p.Product.IdProduct,
                        NmProduct = p.Product.NmProduct,
                        QtProduct = p.QtProduct
                    }).ToList()
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
