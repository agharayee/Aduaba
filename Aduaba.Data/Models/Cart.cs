using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aduaba.Data.Models
{
    public class Cart
    {
        [Key]
        public string Id { get; set; }
        public List<Product> Product { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public Customer Customer { get; set; }
        public string CustomerId { get; set; }
    }
}
