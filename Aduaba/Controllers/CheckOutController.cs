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
        private List<string> CheckingOutItems;
        
        public CheckOutController(IOrderService orderService, IMapper mapper)
        {
            this._orderService = orderService;
            this._mapper = mapper;
            CheckingOutItems = new List<string>();
        }

        [HttpGet("Checkout")]
        public async Task<ActionResult> Checkout([FromBody] List<string> orderItemsId)
        {

            foreach (var item in orderItemsId)
            {
                //HttpContext.Session.SetString("orderItemsId", item);
                CheckingOutItems.Add(item);
            }
            if (orderItemsId == null)
                return BadRequest();
            else
            {
                var Items = await _orderService.GetOrderItems(orderItemsId);
                return Ok(Items);
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
       public async Task<ActionResult> OrderItem([FromBody]List<string> orderItemsId, string orderType)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //var orderItemsId = CheckingOutItems;
            if(orderType == "PayOnDelivery")
            {
                var order = await _orderService.OrderItems(orderItemsId, customerId);
                OrderSucessfulDto sucessful = new OrderSucessfulDto
                {
                    OrderId = order.OrderReferenceNumber
                };
                return Ok($"Your order is successful. you can track your order with this reference number {sucessful.OrderId}");
            }
            else
            {
                return Ok();
            }
            
        }

       
    }
}
