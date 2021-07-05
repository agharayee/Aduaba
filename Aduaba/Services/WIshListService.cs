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
    public class WIshListService : IWishListService
    {
        private readonly ApplicationDbContext _context;

        public WIshListService(ApplicationDbContext context)
        {
            this._context = context;
        }
        public string WishListId { get; set; }
        public async Task AddToWishList(WishList wishList, string productId)
        {
            List<WishListItem> wishListItems = new List<WishListItem>();
            WishListItem products = default;
            WishListItem ExistingProducts = default;
            if (wishList == null) throw new ArgumentNullException(nameof(wishList));
             var existingProductInTheWishList = _context.WishList.Include(w => w.WishListItems).Where(c => c.CustomerId == wishList.CustomerId).ToList();
            if (existingProductInTheWishList == null)
            {
                products = new WishListItem
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = productId,
                    WishListId = wishList.Id,
                };
                
                wishListItems.Add(products);
                wishList.WishListItems = wishListItems;
                await _context.WishList.AddAsync(wishList);

            }
            else
            {

                foreach (var items in existingProductInTheWishList)
                {
                    ExistingProducts = items.WishListItems.FirstOrDefault(w => w.ProductId == productId);
                }
                if(ExistingProducts == null)
                {
                    products = new WishListItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = productId,
                        WishListId = wishList.Id,
                    };

                    wishListItems.Add(products);
                    wishList.WishListItems = wishListItems;
                    await _context.AddAsync(wishList);
                }
                else
                {

                }
            }
           await _context.SaveChangesAsync();
        }

        public IEnumerable<WishListItem> GetCustomerWishListItems(string customerId)
        {
            if (customerId == null) throw new ArgumentNullException(nameof(customerId));
            else
            {
                WishListItem wishListItem = default;
               var productsInWishList = new List<WishListItem>();
               var customerWishList = _context.WishList.Include(c => c.WishListItems).Where(c => c.CustomerId == customerId).ToList();
                if(customerWishList == null)
                {
                    return null;
                }
                else
                {
                    foreach(var item in customerWishList)
                    {
                        foreach(var wishList in item.WishListItems)
                        {
                            wishList.Product = _context.Products.First(c => c.Id == wishList.ProductId);
                            wishListItem = new WishListItem
                            {
                                Id = wishList.Id,
                                Product = wishList.Product
                            };
                            productsInWishList.Add(wishListItem);
                        }           
                    }
                    return productsInWishList;
                }        
            }
        }

        public void RemoveFromWishList(string wishListId, string customerId)
        {
            if (wishListId == null && customerId == null) throw new ArgumentNullException(); 
            else
            {
               var customerWishList = _context.WishList.Include(w=> w.WishListItems).Where(c => c.CustomerId == customerId).ToList();
               foreach(var items in customerWishList)
                {
                    var itemTodelete = items.WishListItems.FirstOrDefault(w => w.Id == wishListId);
                    items.WishListItems.Remove(itemTodelete);
                }
                _context.SaveChanges();
               
            }
        }

        public WIshListService GetWishListOfNotLoginUser(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
               .HttpContext.Session;

            var context = services.GetService<ApplicationDbContext>();

            string wishListId = session.GetString("WishListId") ?? Guid.NewGuid().ToString();

            session.SetString("WishListId", wishListId);
            return new WIshListService(context) { WishListId = wishListId };

        }


       
    }
}
