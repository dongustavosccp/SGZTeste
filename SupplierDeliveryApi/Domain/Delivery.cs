namespace SupplierDeliveryAPI.Domain
{
    public class Delivery
    {
        public int IdDelivery { get; set; }
        public DateTime DtDelivery { get; set; }
        public DeliveryStatus DsStatus { get; set; } = DeliveryStatus.Pending;

        public int IdSupplier { get; set; } = 0;
        public Supplier Supplier { get; set; } = new();

        public List<DeliveryProduct> Products { get; set; } = new();
        public DeliveryAddress Address { get; set; } = new();
    }

    public class DeliveryProduct
    {
        public int IdDelivery { get; set; } = 0;
        public Delivery Delivery { get; set; } = new();

        public int IdProduct { get; set; } = 0;
        public Product Product { get; set; } = new();
        public int QtProduct { get; set; } = 0;
    }

    public class DeliveryAddress
    {
        public int IdDelivery { get; set; } = 0;
        public Delivery Delivery { get; set; } = new();
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