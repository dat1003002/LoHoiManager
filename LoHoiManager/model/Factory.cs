using System;
using System.Collections.Generic;

namespace LoHoiManager.model
{
    public class Factory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<ExportProduct> ExportProducts { get; set; } = new List<ExportProduct>();
    }
}