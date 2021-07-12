using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class RemoveFromCartDto
    {
        [Required(ErrorMessage ="CartItemId is required")]
        public string CartItemId { get; set; }
    }
}
