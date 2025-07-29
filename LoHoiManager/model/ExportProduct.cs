using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoHoiManager.model
{
    public class ExportProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Weight { get; set; }
        public double NetWeight { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ExportTime { get; set; } // Thời gian xuất kho
        public string Barcode { get; set; }
        public ProductStatus Status { get; set; }
        public int SupplierId { get; set; }
        public int PalletId { get; set; }
        public int FactoryId { get; set; }
        public Supplier Supplier { get; set; }
        public Pallet Pallet { get; set; }
        public Factory Factory { get; set; }

    }
}
