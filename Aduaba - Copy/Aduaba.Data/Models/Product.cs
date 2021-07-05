using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aduaba.Data.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ImageUrl { get; set; }
        public string Manufacturer { get; set; }
        public Category Category { get; set; }
        public string CategoryId { get; set; }
        public bool IsLike { get; set; }
        public bool InStock { get; set; }
        public int Quantity { get; set; }
        [Column (TypeName = "Money")]
        public decimal Amount { get; set; }

    }
}
