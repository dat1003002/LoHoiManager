using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoHoiManager.model
{
    public class ProductExportView
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Weight { get; set; }
        public double NetWeight { get; set; }
        public string ExportTime { get; set; } // Định dạng thời gian xuất kho
        public string Barcode { get; set; }
        public string Status { get; set; }
        public string SupplierName { get; set; }
        public string PalletName { get; set; }
        public string FactoryName { get; set; }
    }
}
