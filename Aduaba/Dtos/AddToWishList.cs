using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class AddToWishList
    {
       // public string CustomerId { get; set; }
       [Required]
        public string ProductId { get; set; }
    }
}
