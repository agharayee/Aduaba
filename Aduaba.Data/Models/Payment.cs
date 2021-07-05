using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aduaba.Data.Models
{
    public class Payment
    {
        public string Id { get; set; }
        [Required]
        [Column(TypeName = "Money")]
        public decimal Amount { get; set; }
        [Required]
        public string PaymentReferenceNumber { get; set; }
        public DateTime PaymentDateTime { get; set; } = DateTime.Now;
        [Required]
        public string PaymentStatus { get; set; }
        public virtual Customer Customer { get; set; }
        public string CustomerId { get; set; }
        public virtual Order Order { get; set; }
        public string OrderId { get; set; }
    }
}
