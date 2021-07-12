using Aduaba.Data.Models;
using Aduaba.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Interfaces
{
    public interface ICartService
    {
        List<CartItem> GetCart(string customerId);
        void AddToCart(string productId, int quanity, string customerId);
        void RemoveFromCart(string cartItemId, string customerId);
        void AddToCartWithSession(string productId, int quanity);
        void RemoveFromCartWithSession(string productId);
        CartService GetCartOfNotLoginUser(IServiceProvider services);
        List<CartWithSession> GetShoppingCartItems();
        Task UpdateQuantity(int quanity, string cartItem);


    }
}
