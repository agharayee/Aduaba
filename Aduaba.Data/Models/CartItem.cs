using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aduaba.Data.Models
{
    public class CartItem
    {
        public string Id { get; set; }
        public Product Product { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public Cart Cart { get; set; }
        public string CardId { get; set; }
    }
}
