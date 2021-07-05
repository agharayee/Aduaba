using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aduaba.Data.Models
{
    public class CartWithSession
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public string ShoppingCartId { get; set; }
        public List<Product> Product { get; set; }
        public string ProductId { get; set; }
    }
}
