using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aduaba.Data.Models
{
    public class WishList
    {
        public string Id { get; set; }
        public virtual Customer Customer { get; set; }
        public string CustomerId { get; set; }
        public List<WishListItem> WishListItems { get; set; }
    }
}
