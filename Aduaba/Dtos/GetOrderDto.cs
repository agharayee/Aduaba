using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class GetOrderDto
    {
        public string OrderId { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string Instock { get; set; }
    }
}
