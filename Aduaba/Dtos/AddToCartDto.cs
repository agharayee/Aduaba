﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class AddToCartDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
