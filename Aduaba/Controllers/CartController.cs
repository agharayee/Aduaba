using Aduaba.Dtos;
using Aduaba.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Aduaba.Controllers
{
    [ApiController]
    //[Route("Cart")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        
        string CustomerId { get; set; } 
        public CartController(ICartService service)
        {
            this._service = service;
        }

        
        [HttpPost]
        [Route("RemoveProductFromCart")]
        public IActionResult RemoveFromCart([FromQuery] RemoveFromCartDto cart)
        {
             CustomerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _service.RemoveFromCart(cart.ProductId, CustomerId);
                return Ok("Removed Successfully");
            
            
        }
        [HttpPost]
        [Route("AddToCart")]
        public IActionResult AddToCart([FromBody] AddToCartDto cart)
        {
            if (cart == null) return BadRequest();
            else
            {
                CustomerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    _service.AddToCart(cart.ProductId, cart.Quantity, CustomerId);
                    return Ok("Added to Cart Successfully");
                
               
            }
        }
        [HttpGet]
        [Route("GetCart")]
        public IActionResult GetCustomerCart()
        {
            CustomerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           
                List<GetCartDto> cartDto = new List<GetCartDto>();
                GetCartDto view = default;
                var cart = _service.GetCart(CustomerId);
                decimal sum = default;
                foreach (var item in cart)
                {
                    foreach (var check in item.Product)
                    {
                        sum = check.Amount * item.Quantity;
                        view = new GetCartDto
                        {
                            ProductName = check.Name,
                            UnitPrice = check.Amount,
                            Quantity = item.Quantity,
                            Total = sum,
                            
                        };
                    }

                    cartDto.Add(view);

                }
                return Ok(cartDto);
            
        }
    }
}
