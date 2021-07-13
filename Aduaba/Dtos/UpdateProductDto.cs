using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class UpdateProductDto 
    {
 
        public string Name { get; set; }

        public string CategoryId { get; set; }
       
        public string ShortDescription { get; set; }
    
        public string LongDescription { get; set; }
        
        public string ImageUrl { get; set; }
       
        public string Manufacturer { get; set; }
        public bool IsLike { get; set; }
   
        public bool InStock { get; set; }
       
        public int Quantity { get; set; }
       
        public decimal Amount { get; set; }

        public bool BestSelling { get; set; }
        public bool FeaturedProduct { get; set; }
    }
}
