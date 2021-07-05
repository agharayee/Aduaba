using Aduaba.Data.Models;
using Aduaba.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Interfaces
{
    public interface IOrderService
    {
        Task<List<CheckOutDto>> GetOrderItems(List<string> orderItems);
        Task<ShippingAddress> GetCustomerShippingAddress(string customerId);
        Task ProcessPayment();
        Task<Order> OrderItems(List<string> orderItems, string customerId);
        Task<List<Order>> OrderItems(string customerId);
    }
}
