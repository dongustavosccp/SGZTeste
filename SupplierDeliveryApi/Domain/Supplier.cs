namespace SupplierDeliveryAPI.Domain
{
    public class Supplier
    {
        public int IdSupplier { get; set; } = 0;
        public string NmSupplier { get; set; } = "";
        public List<Delivery> Deliveries { get; set; } = new();
    }
}
