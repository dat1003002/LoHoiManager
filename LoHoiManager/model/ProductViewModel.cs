namespace LoHoiManager.model
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Weight { get; set; }
        public string PalletName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public double DefaultWeight { get; set; }
        public DateTime CreateTime { get; set; }
        public ProductStatus Status { get; set; }
        public string StatusDisplay { get; set; }
    }
}