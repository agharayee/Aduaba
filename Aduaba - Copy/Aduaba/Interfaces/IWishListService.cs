using Aduaba.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Interfaces
{
    public interface IWishListService
    {
        Task AddToWishList(WishList wishList, string productId);
        IEnumerable<WishListItem> GetCustomerWishListItems(string customerId);
        void RemoveFromWishList(string wishListId, string customerId);
    }
}
