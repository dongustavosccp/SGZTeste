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
        private readonly IProductRepository _productRepository;
        private readonly INotifier _notifier;
        public DeliveryService(IDeliveryRepository repository, IProductRepository productRepository, INotifier notifier)
        {
            _repository = repository;
            _productRepository = productRepository;
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
                    Products = new()
                };


                foreach (DeliveryProductRequest item in delivery.Products)
                {

                    var product = await _productRepository.GetProductById(item.IdProduct);
                    if (product == null || product.IdProduct <= 0)
                    {
                        _notifier.AddNotification(new ErrorMessage($"Product with ID {item.IdProduct} not found."));
                        return new();
                    }

                    if (item.QtProduct <= 0)
                    {
                        _notifier.AddNotification(new ErrorMessage($"Quantity for product {item.IdProduct} must be greater than zero."));
                        return new();
                    }

                    domainDelivery.Products.Add(new DeliveryProduct
                    {
                        IdProduct = item.IdProduct,
                        QtProduct = item.QtProduct,
                        Product = product
                    });
                }


                Delivery savedDelivery = await _repository.AddDelivery(domainDelivery);

                if (savedDelivery == null || savedDelivery.IdDelivery <= 0)
                {
                    _notifier.AddNotification(new ErrorMessage("Failed to save delivery."));
                    return new();
                }

                var response = await _repository.GetDeliveryById(savedDelivery.IdDelivery);

                return new()
                {
                    IdDelivery = response.IdDelivery,
                    DtDelivery = response.DtDelivery,
                    DsStatus = response.DsStatus,
                    Supplier = new()
                    {
                        IdSupplier = response.Supplier.IdSupplier,
                        NmSupplier = response.Supplier.NmSupplier
                    },
                    Address = new()
                    {
                        Street = response.Address.Street,
                        Number = response.Address.Number,
                        City = response.Address.City,
                        State = response.Address.State,
                        ZipCode = response.Address.ZipCode
                    },
                    Products = response.Products.Select(p => new DeliveryProductResponse
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
