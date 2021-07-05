using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class RemoveFromWishListDto
    {
        public string WishListItemId { get; set; }
        public string CustomerId { get; set; }

    }
}
