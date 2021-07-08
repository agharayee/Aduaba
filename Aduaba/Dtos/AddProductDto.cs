using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class AddProductDto
    {
       [Required(ErrorMessage = "Name is required" )]
        public string Name { get; set; }
        [Required(ErrorMessage = "Category id is required")]
        public string CategoryId { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(100)]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500)]
        public string LongDescription { get; set; }
        [Required(ErrorMessage = "Image is required")]
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Manufacturer's name is required")]
        public string Manufacturer { get; set; }
        public bool IsLike { get; set; }
        [Required]
        public bool InStock { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        [Column(TypeName = "Money")]
        public decimal Amount { get; set; }

        public bool BestSelling { get; set; }
        public bool FeaturedProduct { get; set; }

    }
}
