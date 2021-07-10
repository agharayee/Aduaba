using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aduaba.Data.Models
{
    public class CustomerOTP
    {
        public string Id { get; set; }
        public Customer Customer { get; set; }
        public string CustomerId { get; set; }
        public int Otp { get; set; }
    }
}
