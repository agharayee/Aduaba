using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aduaba.Data.Models
{
    public class ShippingAddress
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternativePhoneNumber { get; set; }
        public string Address { get; set; }
        public string AdditionalInformation { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string LandMark { get; set; }
        public Customer Customer { get; set; }
        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
    }
}
