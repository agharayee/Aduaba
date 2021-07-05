using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class GetShippingAddressDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternativePhoneNumber { get; set; }
        public string Address { get; set; }
        public string AdditionalInformation { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string LandMark { get; set; }
    }
}
