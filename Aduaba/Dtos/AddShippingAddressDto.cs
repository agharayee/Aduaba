using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class AddShippingAddressDto
    {
        [Required(ErrorMessage ="Full Name is Required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "PhoneNumber is Required")]
        public string PhoneNumber { get; set; }
        public string AlternativePhoneNumber { get; set; }
        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }
        public string AdditionalInformation { get; set; }
        [Required(ErrorMessage = "State is Required")]
        public string State { get; set; }
        [Required(ErrorMessage = "City is Required")]
        public string City { get; set; }
        [Required(ErrorMessage = "LandMark is Required")]
        public string LandMark { get; set; }
        public string CustomerId { get; set; }
    }
}
