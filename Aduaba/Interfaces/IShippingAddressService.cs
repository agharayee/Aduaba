using Aduaba.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Interfaces
{
    public interface IShippingAddressService
    {
        Task<ShippingAddress> GetCustomerShippingAddress(string customerId);
        Task AddShippingAddress(ShippingAddress shippingAddress);
        Task EditShippingAddress(ShippingAddress shippingAddress);
    }
}
