using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class RemoveFromWishListDto
    {
        [Required(ErrorMessage = "WishListItemId is required")]
        public string WishListItemId { get; set; }
        //public string CustomerId { get; set; }

    }
}
