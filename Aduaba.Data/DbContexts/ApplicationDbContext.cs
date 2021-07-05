using Aduaba.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aduaba.Data.DbContexts
{
   public class ApplicationDbContext : IdentityDbContext<Customer>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories  { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        
        public DbSet<ShippingAddress> ShippingAddress { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<WishListItem> WishListItems { get; set; }
        public DbSet<CartWithSession> CartWithSessions { get; set; }
    }
}
