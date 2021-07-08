using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class UpdateQuantityDto
    {
        public string CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
