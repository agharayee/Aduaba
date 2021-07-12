using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class GetCategoryDto : AddCategoryDto
    {
        public string Id { get; set; }
        public int Length { get; set; }
    }
}
