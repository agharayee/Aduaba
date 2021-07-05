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
        List<Product> productsInCart { get; set; }
        Cart customerCart { get; set; }
        public string ShoppingCartId { get; set; }

        public CartService(ApplicationDbContext context)
        {
            productsInCart = new List<Product>();
            this._context = context;
        }

        public void AddToCart(string productId, int quanity, string customerId)
        {
            var existingProductInTheCart = _context.Carts.Where(c => c.CustomerId == customerId).ToList();
            if (existingProductInTheCart == null)
            {
                Cart cart = new Cart
                {
                    ProductId = productId,
                    Quantity = quanity,
                    CustomerId = customerId,
                    Id = Guid.NewGuid().ToString(),
                    //TotalAmount = GetShoppingCartTotal(customerId),
                };
                _context.Carts.Add(cart);
            }
            else
            {
                var existingProduct = existingProductInTheCart.FirstOrDefault(p => p.ProductId == productId);
                if (existingProduct == null)
                {
                    Cart cart = new Cart
                    {
                        ProductId = productId,
                        Quantity = quanity,
                        CustomerId = customerId,
                        Id = Guid.NewGuid().ToString(),
                        //TotalAmount = GetShoppingCartTotal(customerId),
                    };
                    _context.Carts.Add(cart);
                }
                else
                {
                    existingProduct.Quantity += quanity;
                }
            }
            _context.SaveChanges();
        }

        public List<Cart> GetCart(string customerId)
        {
            List<Cart> productInCart = new List<Cart>();
            productInCart = _context.Carts.Where(c => c.CustomerId == customerId).ToList();
            foreach (var products in productInCart)
            {
                products.Product = _context.Products.Where(p => p.Id == products.ProductId).ToList();
            }
            return productInCart;
        }

        public void RemoveFromCart(string productId, string customerId)
        {
            var foundProductInCart = _context.Carts.Where(c => c.CustomerId == customerId);
            var productToRemove = foundProductInCart.FirstOrDefault(p => p.ProductId == productId);
            _context.Carts.Remove(productToRemove);
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

        //public List<CartWithSession> GetShoppingCartItems()
        //{
        //    List<CartWithSession> productInCart = new List<CartWithSession>();
        //    productInCart = _context.CartWithSessions.Where(c => c.ShoppingCartId == ShoppingCartId ).ToList();
        //    foreach (var products in productInCart)
        //    {
        //        products.Product = _context.Products.Where(p => p.Id == products.ProductId).ToList();
        //    }
        //    return productInCart;
        //}

        public void RemoveFromCartWithSession(string productId)
        {
            var foundProductInCart = _context.CartWithSessions.Where(c => c.ShoppingCartId == ShoppingCartId);
            var productToRemove = foundProductInCart.FirstOrDefault(p => p.ProductId == productId);
            _context.CartWithSessions.Remove(productToRemove);
            _context.SaveChanges();
        }
    }
}
