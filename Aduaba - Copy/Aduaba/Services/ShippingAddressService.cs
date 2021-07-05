using Aduaba.Data.DbContexts;
using Aduaba.Data.Models;
using Aduaba.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Services
{
    public class ShippingAddressService : IShippingAddressService
    {
        private readonly ApplicationDbContext _context;

        public ShippingAddressService(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task AddShippingAddress(ShippingAddress shippingAddress)
        {
            if (shippingAddress == null) throw new ArgumentNullException(nameof(shippingAddress));
            else 
            {
                await _context.ShippingAddress.AddAsync(shippingAddress);
                await _context.SaveChangesAsync();
            }
        }

        public async Task EditShippingAddress(ShippingAddress shippingAddress)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<ShippingAddress> GetCustomerShippingAddress(string customerId)
        {
            if (customerId == null) throw new ArgumentNullException(nameof(customerId));
            else
            {
                var customerShippingAddress = await _context.ShippingAddress.FirstOrDefaultAsync(c => c.CustomerId == customerId);
                return customerShippingAddress;
            }
        }

        
    }
}
