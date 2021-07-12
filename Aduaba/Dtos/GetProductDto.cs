using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class GetProductDto : AddProductDto
    {
        public string ProductId { get; set; }
        public string IsAvailable{ get; set; }
    }
}
