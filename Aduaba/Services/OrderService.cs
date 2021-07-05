using Aduaba.Data.DbContexts;
using Aduaba.Data.Models;
using Aduaba.Dtos;
using Aduaba.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Services
{
    public class OrderService : IOrderService
    {
        private readonly IShippingAddressService _shippingAddress;
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public OrderService(IShippingAddressService shippingAddress, ApplicationDbContext context, IConfiguration configuration)
        {
            this._shippingAddress = shippingAddress;
            this._context = context;
            _configuration = configuration;
        }
        public async Task<ShippingAddress> GetCustomerShippingAddress(string customerId)
        {
            var customerShippingAddress = await _shippingAddress.GetCustomerShippingAddress(customerId);
            return customerShippingAddress;
        }

        public async Task<List<CheckOutDto>> GetOrderItems(List<string> orderItemId)
        {
            List<CheckOutDto> cartItems = new List<CheckOutDto>();
            CheckOutDto checkOut = default;
            string InStock = default;
            foreach (var item in orderItemId)
            {
                var foundItems = await _context.CartItems.FirstOrDefaultAsync(c => c.Id == item);
                var productInFoundItem = await _context.Products.FirstOrDefaultAsync(c => c.Id == foundItems.ProductId);
                foundItems.Product = productInFoundItem;

                if (foundItems.Product.InStock)
                    InStock = "In Stock";
                else
                    InStock = "Not in shock";

                checkOut = new CheckOutDto
                {
                    ProductName = foundItems.Product.Name,
                    Amount = foundItems.Product.Amount,
                    ManufacturerName = foundItems.Product.Manufacturer,
                    Quantity = foundItems.Quantity,
                    Instock = InStock
                };


                cartItems.Add(checkOut);
            }
            return cartItems;
        }

        public Task<List<Order>> OrderItems(string customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> OrderItems(List<string> orderItems, string customerId)
        {
            Order order = default;
            foreach (var item in orderItems)
            {
                List<CartItem> cartItems = new List<CartItem>();
                var foundItems = await _context.CartItems.FirstOrDefaultAsync(c => c.Id == item);
                var productInFoundItem = await _context.Products.FirstOrDefaultAsync(c => c.Id == foundItems.ProductId);
                foundItems.Product = productInFoundItem;
                cartItems.Add(foundItems);
                var customer = await _context.Users.FirstOrDefaultAsync(c => c.Id == customerId);
                var shipping = await GetCustomerShippingAddress(customerId);
                order = new Order
                {
                    OrderItems = cartItems,
                    OrderDate = DateTime.Now,
                    Id = Guid.NewGuid().ToString(),
                    OrderType = "Pay On Deliver",
                    OrderReferenceNumber = Guid.NewGuid().ToString(),
                    ShippingAddress = shipping,
                    PaymentStatus = false,
                    Customer = customer,
                    TotalAmountToPay = await CalculateTotalBilling(orderItems),
                    OrderStatus = new OrderStatus
                    {
                        Id = Guid.NewGuid().ToString(),
                        Status = "Shipping In Progress"
                    }
                };
                await _context.Orders.AddAsync(order);
            }
            await _context.SaveChangesAsync();
            return order;
        }


        public async Task ChangeOrderStatus(string orderId, string orderStatus)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(a => a.Id == orderId);
            order.OrderStatus = new OrderStatus
            {
                Status = orderStatus
            };
            await _context.SaveChangesAsync();
        }

        private async Task<decimal> CalculateTotalBilling(List<string> cartItems)
        {
            decimal SubTotal = default;
            foreach (var items in cartItems)
            {
                var foundItems = await _context.CartItems.FirstOrDefaultAsync(c => c.Id == items);
                var productInFoundItem = await _context.Products.FirstOrDefaultAsync(c => c.Id == foundItems.ProductId);
                foundItems.Product = productInFoundItem;
                decimal total = foundItems.Product.Amount * foundItems.Quantity;
                SubTotal += total;
            }
            return SubTotal;
        }




        public async Task PayOnDelivery()
        {

        }



        public Task ProcessPayment()
        {
            throw new NotImplementedException();
        }
    }
}
