using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoHoiManager.model
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Manhacungcap { get; set; }
        public int? total { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        // Danh sách sản phẩm thuộc nhà cung cấp
        public List<Product> Products { get; set; } = new List<Product>();
        public List<ExportProduct> ExportProducts { get; set; } = new List<ExportProduct>();
    }
}   