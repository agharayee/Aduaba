using Aduaba.Data.DbContexts;
using Aduaba.Data.Models;
using Aduaba.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        List<Product> ProductsInCart { get; set; }
        Cart customerCart { get; set; }
        public string ShoppingCartId { get; set; }

        public CartService(ApplicationDbContext context)
        {
            ProductsInCart = new List<Product>();
            this._context = context;
        }

        public void AddToCart(string productId, string customerId, int quanity = 1)
        {
            List<CartItem> CartItems = new List<CartItem>();
            CartItem products = default;
            CartItem ExistingProducts = default;
            if (productId == null) throw new ArgumentNullException(nameof(productId));
            var existingProductInCart = _context.Carts.Include(w => w.CartItem).Where(c => c.CustomerId == customerId).ToList();
            var product = _context.Products.FirstOrDefault(c => c.Id == productId);

            if (existingProductInCart == null)
            {
                products = new CartItem
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = productId,
                    CardId = Guid.NewGuid().ToString(),
                    Quantity = quanity,
                    CartItemTotal = product.Amount * quanity
                };

                CartItems.Add(products);
                var CartToBeAdded = new Cart
                {
                    CartItem = CartItems,
                    CustomerId = customerId,
                };

                _context.Carts.Add(CartToBeAdded);

            }
            else
            {
                foreach (var items in existingProductInCart)
                {
                    ExistingProducts = items.CartItem.FirstOrDefault(w => w.ProductId == productId);
                    if (ExistingProducts != null) break;

                }
                if (ExistingProducts == null)
                {
                    string cartId = Guid.NewGuid().ToString();
                    var CartToBeAdded = new Cart
                    {
                        Id = cartId,
                        CartItem = CartItems,
                        CustomerId = customerId,
                    };
                     products = new CartItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = productId,
                        Quantity = quanity,
                        CardId = cartId,
                        CartItemTotal = product.Amount * quanity
                    };

                    CartItems.Add(products);


                    _context.Carts.Add(CartToBeAdded);
                }

                else
                {
                    ExistingProducts.Quantity += quanity;
                    ExistingProducts.CartItemTotal = product.Amount * ExistingProducts.Quantity;
                }
            }
            _context.SaveChanges();
        }


        public List<CartItem> GetCart(string customerId)
        {
            if (customerId == null) throw new ArgumentNullException(nameof(customerId));
            else
            {
                CartItem wishListItem = default;
                var productsInWishList = new List<CartItem>();
                var customerWishList = _context.Carts.Include(c => c.CartItem).Where(c => c.CustomerId == customerId).ToList();
                if (customerWishList == null)
                {
                    return null;
                }
                else
                {
                    foreach (var item in customerWishList)
                    {
                        foreach (var cart in item.CartItem)
                        {
                            cart.Product = _context.Products.First(c => c.Id == cart.ProductId);
                            wishListItem = new CartItem
                            {
                                Id = cart.Id,
                                Product = cart.Product,
                                Quantity = cart.Quantity,


                            };
                            productsInWishList.Add(wishListItem);
                        }
                    }
                    return productsInWishList;

                }
            }
        }

        public void RemoveFromCart(string cartItemId, string customerId)
        {
            var customerWishList = _context.Carts.Include(w => w.CartItem).Where(c => c.CustomerId == customerId).ToList();
            foreach (var items in customerWishList)
            {
                var itemTodelete = items.CartItem.FirstOrDefault(w => w.Id == cartItemId);
                items.CartItem.Remove(itemTodelete);
            }
            _context.SaveChanges();
        }

        public CartService GetCartOfNotLoginUser(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
               .HttpContext.Session;

            var context = services.GetService<ApplicationDbContext>();

            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);
            return new CartService(context) { ShoppingCartId = cartId };

        }

        public void AddToCartWithSession(string productId, int quanity)
        {
            var existingProductInTheCart = _context.CartWithSessions.Where(c => c.ShoppingCartId == ShoppingCartId).ToList();
            if (existingProductInTheCart == null)
            {
                CartWithSession cart = new CartWithSession
                {
                    ProductId = productId,
                    Quantity = quanity,
                    ShoppingCartId = ShoppingCartId,
                    Id = Guid.NewGuid().ToString(),
                    //TotalAmount = GetShoppingCartTotal(customerId),
                };
                _context.CartWithSessions.Add(cart);
            }
            else
            {
                var existingProduct = existingProductInTheCart.FirstOrDefault(p => p.ProductId == productId);
                if (existingProduct == null)
                {
                    CartWithSession cart = new CartWithSession
                    {
                        ProductId = productId,
                        Quantity = quanity,
                        ShoppingCartId = ShoppingCartId,
                        Id = Guid.NewGuid().ToString(),
                        //TotalAmount = GetShoppingCartTotal(customerId),
                    };
                    _context.CartWithSessions.Add(cart);
                }
                else
                {
                    existingProduct.Quantity += quanity;
                }
            }
            _context.SaveChanges();
        }

        public List<CartWithSession> GetShoppingCartItems()
        {
            List<CartWithSession> productInCart = new List<CartWithSession>();
            productInCart = _context.CartWithSessions.Where(c => c.ShoppingCartId == ShoppingCartId).ToList();
            foreach (var products in productInCart)
            {
                products.Product = _context.Products.Where(p => p.Id == products.ProductId).ToList();
            }
            return productInCart;
        }

        public void RemoveFromCartWithSession(string productId)
        {
            var foundProductInCart = _context.CartWithSessions.Where(c => c.ShoppingCartId == ShoppingCartId);
            var productToRemove = foundProductInCart.FirstOrDefault(p => p.ProductId == productId);
            _context.CartWithSessions.Remove(productToRemove);
            _context.SaveChanges();
        }

        public async Task UpdateQuantity(int quanity, string cartItem)
        {
            var foundItem = _context.CartItems.FirstOrDefault(c => c.Id == cartItem);
            foundItem.Quantity = quanity;
            await _context.SaveChangesAsync();
        }
    }
}
