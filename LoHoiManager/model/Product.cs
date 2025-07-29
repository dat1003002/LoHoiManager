using System;

namespace LoHoiManager.model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Weight { get; set; }
        public double NetWeight { get; set; }
        public string Barcode { get; set; } // Mã vạch
        public ProductStatus Status { get; set; } // Trạng thái
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int SupplierId { get; set; }
        public int PalletId { get; set; }
        public int FactoryId { get; set; }
        public Pallet Pallet { get; set; }
        public Supplier Supplier { get; set; }
        public Factory Factory { get; set; }
    }
}