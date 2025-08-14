using SupplierDeliveryAPI.Domain;

namespace SupplierDeliveryAPI.Models
{
    public class DeliveryProductRequest
    {
        public int IdProduct { get; set; } = 0;
        public int QtProduct { get; set; } = 0;
    }

    public class DeliveryAddressRequest
    {
        public string Street { get; set; } = "";
        public string Number { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string ZipCode { get; set; } = "";
    }

    public class DeliveryRequest
    {
        public int IdDelivery { get; set; } = 0;
        public DateTime DtDelivery { get; set; }
        public DeliveryStatus DsStatus { get; set; } = DeliveryStatus.Pending;
        public int IdSupplier { get; set; } = 0;
        public DeliveryAddressRequest Address { get; set; } = new();
        public List<DeliveryProductRequest> Products { get; set; } = new();
    }

    public class DeliveryProductResponse
    {
        public int IdProduct { get; set; } = 0;
        public string NmProduct { get; set; } = "";
        public int QtProduct { get; set; } = 0;
    }

    public class DeliveryAddressResponse
    {
        public string Street { get; set; } = "";
        public string Number { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string ZipCode { get; set; } = "";
    }

    public class DeliveryResponse
    {
        public int IdDelivery { get; set; } = 0;
        public DateTime DtDelivery { get; set; }
        public DeliveryStatus DsStatus { get; set; } = DeliveryStatus.Pending;
        public SupplierDTO Supplier { get; set; } = new();
        public DeliveryAddressResponse Address { get; set; } = new();
        public List<DeliveryProductResponse> Products { get; set; } = new();
    }
}