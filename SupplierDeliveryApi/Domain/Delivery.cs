namespace SupplierDeliveryAPI.Domain
{
    public class Delivery
    {
        public int IdDelivery { get; set; }
        public DateTime DtDelivery { get; set; }
        public DeliveryStatus DsStatus { get; set; } = DeliveryStatus.Pending;

        public int IdSupplier { get; set; } = 0;
        public Supplier Supplier { get; set; }

        public List<DeliveryProduct> Products { get; set; }
        public DeliveryAddress Address { get; set; }
    }

    public class DeliveryProduct
    {
        public int IdDelivery { get; set; } = 0;
        public Delivery Delivery { get; set; }

        public int IdProduct { get; set; } = 0;
        public Product Product { get; set; }
        public int QtProduct { get; set; } = 0;
    }

    public class DeliveryAddress
    {
        public int IdDelivery { get; set; } = 0;
        public Delivery Delivery { get; set; }
        public string Street { get; set; } = "";
        public string Number { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string ZipCode { get; set; } = "";
    }

    public enum DeliveryStatus
    {
        Pending,
        Shipped,
        Delivered,
        Canceled
    }
}