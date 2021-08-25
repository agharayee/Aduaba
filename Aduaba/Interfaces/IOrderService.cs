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
        Task<OrderStatus> TrackOrder(string trackingId);
        Task ChangeOrderStatus(string orderId, string orderStatus);
        Task<Order> GetOrderItems(List<string> orderItemId, string customerId);
        Task<ShippingAddress> GetCustomerShippingAddress(string customerId);
        Task ProcessPayment();
        Task<List<Order>> OrderItemsForInstancePayment(string customerId);
        Task<Order> OrderItemsForPayOnDelivery(string orderId, string customerId);
    }
}
