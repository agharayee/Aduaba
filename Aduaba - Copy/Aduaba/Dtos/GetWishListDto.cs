using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Dtos
{
    public class GetWishListDto
    {
        public string WishListItemId { get; set; }
        public string ProductName { get; set; }
        public string Manufacturers { get; set; }
        public decimal Amount { get; set; }
        public bool IsAvailable { get; set; }

    }
}
