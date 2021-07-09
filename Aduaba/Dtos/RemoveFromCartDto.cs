using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class RemoveFromCartDto
    {
        [Required(ErrorMessage ="ProductId is required")]
        public string ProductId { get; set; }
    }
}
