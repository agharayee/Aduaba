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
            public IActionResult RemoveFromCart([FromBody] RemoveFromCartDto cart)
            {
                CustomerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _service.RemoveFromCart(cart.CartItemId, CustomerId);
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

                    sum = item.Product.Amount * item.Quantity;
                    view = new GetCartDto
                    {
                        ProductName = item.Product.Name,
                        UnitPrice = item.Product.Amount,
                        Quantity = item.Quantity,
                        Total = sum,
                        CartItemId = item.Id
                    };


                    cartDto.Add(view);

                }
                return Ok(cartDto);

            }

            [HttpPost]
            [Route("UpdateQuantity")]
            public async Task<ActionResult> UpdateQuantityOfACartItem(UpdateQuantityDto quantity)
            {
                if (quantity == null) return BadRequest();
                else
                {
                    await _service.UpdateQuantity(quantity.Quantity, quantity.CartItemId);
                    return Ok("Item quantity changed Successfully");

                }
            }
        }
}
