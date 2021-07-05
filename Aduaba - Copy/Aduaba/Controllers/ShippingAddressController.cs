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
    [Route("ShippingAddress")]
    public class ShippingAddressController : ControllerBase
    {
        private readonly IShippingAddressService _service;
        private readonly IMapper _mapper;
        string customerId { get; set; }

        public ShippingAddressController(IShippingAddressService service, IMapper mapper)
        {
            this._service = service;
            this._mapper = mapper;
        }
        [HttpPost]
        
        [Route("AddAddress")]
        public async Task<ActionResult> AddShippingAddress([FromBody]AddShippingAddressDto address)
        {
            customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (address == null) return BadRequest();
            else
            {
                address.CustomerId = customerId;
                var mappedAddress = _mapper.Map<ShippingAddress>(address);
                mappedAddress.Id = Guid.NewGuid().ToString();
                await _service.AddShippingAddress(mappedAddress);
                return Created("Address added Successfully", new {FullName = mappedAddress.FullName, PhoneNumber = mappedAddress.PhoneNumber, mappedAddress.CustomerId });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetShippingAddress()
        {
            customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customerId == null) return BadRequest();
            else
            {
                var customerShippingAddress = await _service.GetCustomerShippingAddress(customerId);
                var address = _mapper.Map<GetShippingAddressDto>(customerShippingAddress);
                if(address == null)
                {
                    return Ok("No Shipping address found");
                }
                else
                return Ok(address);
            }
        }
        [HttpPut]
        [Route("EditShippingAddress")]
        public async Task<IActionResult> EditShippingAddress([FromBody] UpdateShippingAddressDto address)
        {
            customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            address.CustomerId = customerId;
            var foundShippingAddress = await _service.GetCustomerShippingAddress(customerId);
            if(foundShippingAddress == null)
            {
                return NotFound();
            }else
            {
                _mapper.Map(address, foundShippingAddress);
                await _service.EditShippingAddress(foundShippingAddress);
                return NoContent();
            }
        }
    }
}
