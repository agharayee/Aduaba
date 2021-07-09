using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class AddCategoryDto
    {
        [Required(ErrorMessage ="CategoryName is Required")]
        public string Name { get; set; }
    }
}
