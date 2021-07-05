using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class AddShippingAddressDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string AlternativePhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        public string AdditionalInformation { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string LandMark { get; set; }
        public string CustomerId { get; set; }
    }
}
