using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aduaba.Data.Models
{
    public class Order
    {
        public string Id { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ShippingAddress ShippingAddress { get; set; }
        public string ShippingAddressId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        //public bool PaymentStatus { get; set; }
        public string OrderReferenceNumber { get; set; }
        public string OrderType { get; set; }
        public List<CartItem> OrderItems { get; set; } = new List<CartItem>();
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "Money")]
        public decimal TotalAmountToPay { get; set; }
    }
}
