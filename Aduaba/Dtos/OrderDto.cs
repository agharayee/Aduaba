using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class OrderDto
    {
        [Required(ErrorMessage ="OrderItemId is required")]
        public string OrderItemId { get; set; }
        [Required(ErrorMessage ="OrderType is Required")]
        public string OrderType { get; set; }
    }
}
