using Aduaba.Data.DbContexts;
using Aduaba.Data.Models;
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
    [Route("WishList")]
    public class WishListController : ControllerBase 
    {
        private readonly IWishListService _service;
        private readonly IMapper _mapper;
        string customerId { get; set; }

        public WishListController(IWishListService service, IMapper mapper)
        {
            this._service = service;
            this._mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetCustomerWishList()
        {
            customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null) return BadRequest();
            else
            {
                var customerWishList = _service.GetCustomerWishListItems(customerId);
                if(customerWishList == null)
                {
                    return BadRequest("User not found");
                }
                else if (!customerWishList.Any())             
                    return Ok("No Wish found");              
                else
                {
                    var productInWishList = new List<GetWishListDto>();
                    foreach(var item in customerWishList)
                    {
                        GetWishListDto customerWishes = new GetWishListDto
                        {
                            WishListItemId = item.Id,
                            ProductName = item.Product.Name,
                            Amount = item.Product.Amount,
                            Manufacturers = item.Product.Manufacturer,
                            IsAvailable = item.Product.InStock
                        };

                        productInWishList.Add(customerWishes);
                    }
                    return Ok(productInWishList);
                }
                   
            }
        }

        [HttpPost]
        [Route("AddToWishList")]
        [Authorize]
        public async Task<ActionResult> AddToWishCustomerList([FromBody] AddToWishList wishList)
        {
            if (wishList == null) return BadRequest();
            else
            {
                customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                wishList.CustomerId = customerId;
                var customerWishlist = _mapper.Map<WishList>(wishList);
                customerWishlist.Id = Guid.NewGuid().ToString();
                await _service.AddToWishList(customerWishlist, wishList.ProductId);

                return Created("Successfully added to wishlist", new { wishListId = customerWishlist.Id });
            }
        }
        [HttpPost]
        [Route("Remove")]
        [Authorize]
        public IActionResult RemoveFromWishList([FromBody] RemoveFromWishListDto removeFromWishList)
        {
            customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (removeFromWishList.WishListItemId == null && removeFromWishList.CustomerId == null) return BadRequest();
            else
            {
                _service.RemoveFromWishList(removeFromWishList.WishListItemId, customerId);
                return NoContent();
            }
        }
    }
}
