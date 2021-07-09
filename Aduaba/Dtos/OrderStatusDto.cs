using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class OrderStatusDto
    {
        [Required(ErrorMessage ="OrderItemId is required")]
        public string OrderItemId { get; set; }
        [Required(ErrorMessage = "OrderStatus is required")]
        public string OrderStatus { get; set; }
    }
}
