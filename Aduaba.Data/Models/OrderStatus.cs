using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aduaba.Data.Models
{
    public class OrderStatus
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public bool PaymentStatus { get; set; }
    }
}
