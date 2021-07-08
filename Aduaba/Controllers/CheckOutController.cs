using Aduaba.Dtos;
using Aduaba.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Aduaba.Controllers
{

    [ApiController]
    [Authorize]
    public class CheckOutController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
      
        
        public CheckOutController(IOrderService orderService, IMapper mapper)
        {
            this._orderService = orderService;
            this._mapper = mapper;
       
        }

        [HttpGet("Checkout")]
        public async Task<ActionResult> Checkout([FromBody] List<string> orderItemsId)
        {

            
            if (orderItemsId == null)
                return BadRequest();
            else
            {
                string InStock = default;
                List<GetOrderDto> orderDto = new List<GetOrderDto>();
                var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var Items = await _orderService.GetOrderItems(orderItemsId, customerId);
                foreach(var cartItem in Items.OrderItems)
                {
                    if (cartItem.Product.InStock == true) InStock = "In Stock";
                    else InStock = "Out of Stock";
                    var orderItem = new GetOrderDto
                    {
                        OrderId = Items.Id,
                        Instock =InStock,
                        ProductImage = cartItem.Product.ImageUrl,
                        ManufacturerName = cartItem.Product.Manufacturer,
                        ProductName = cartItem.Product.Name,
                        Quantity = cartItem.Quantity,
                        Total = cartItem.CartItemTotal
                    };
                    orderDto.Add(orderItem);
                }
                return Ok(orderDto);
            }
        }

        [HttpGet]
        [Route("ShippingAddress")]
        public async Task<ActionResult> GetShippingAddress()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shippingaddress =  await _orderService.GetCustomerShippingAddress(customerId);
            if (shippingaddress == null)
                return Ok("No Shipping Address found for this User");
            else
            {
                var mappedShippingAddress = _mapper.Map<GetShippingAddressDto>(shippingaddress);
                return Ok(mappedShippingAddress);
            }
        }

        [HttpPost]
        [Route("PayOnDelivery")]
       public async Task<ActionResult> OrderItem([FromBody] OrderDto order)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //var orderItemsId = CheckingOutItems;
            if(order.OrderType == "PayOnDelivery")
            {
                var customerOrder = await _orderService.OrderItems(order.OrderItemId, customerId);
                OrderSucessfulDto sucessful = new OrderSucessfulDto
                {
                    OrderId = customerOrder.OrderReferenceNumber
                };
                return Ok($"Your order is successful. you can track your order with this reference number {sucessful.OrderId}");
            }
            else
            {
                return Ok();
            }
            
        }

        [HttpPost]
        [Route("OrderStatus")]
        public async Task<ActionResult> OrderStatus([FromBody] OrderStatusDto orderStatus)
        {
            if (orderStatus == null) return BadRequest();
            else
            {
                await _orderService.ChangeOrderStatus(orderStatus.OrderItemId, orderStatus.OrderStatus);
                return Ok("OrderStatus Changed Successfully");
            }

        }

        [HttpGet]
        [Route("OrderStatus")]
        public async Task<ActionResult> OrderStatus([FromQuery] string orderReferenceNumber)
        {
            if (orderReferenceNumber == null) return BadRequest();
            else
            {
                var orderStatus =await _orderService.TrackOrder(orderReferenceNumber);
                if (orderStatus == null) return BadRequest("No order found with this reference Number");
                else return Ok(orderStatus);             
            }

        }


    }
}
