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
       // [Route("Customer")]
        public class CustomerController : ControllerBase
        {
            private readonly ICustomerService _service;
            private readonly IMapper _mapper;

            public CustomerController(ICustomerService service, IMapper mapper)
            {
                this._service = service;
                this._mapper = mapper;
            }

            //[HttpPost]
            //[Route("addCustomer")]
            //public IActionResult AddCustomer([FromBody] AddCustomerDto customer)
            //{
            //    if (customer == null) return BadRequest();
            //    else
            //    {
            //        var mappedToCustomer = _mapper.Map<Customer>(customer);
            //        mappedToCustomer.Id = Guid.NewGuid().ToString();
            //        _service.AddCustomer(mappedToCustomer);
            //        return Created("Account Created Sucessfully", new { customerId = mappedToCustomer.Id, CustomerName = $"{mappedToCustomer.FirstName} {mappedToCustomer.LastName}" });
            //    }
            //}

        ////To add Customer you now login
        //[HttpGet]
        //[Route("customerId")]
        //public IActionResult GetCustomerById([FromQuery] string customerId)
        //{
        //    if (customerId == null)
        //    {
        //        return BadRequest();
        //    }
        //    else
        //    {
        //        if (_service.CustomerExists(customerId))
        //        {
        //            var customerFound = _service.GetCustomerById(customerId);
        //            var mappedCustomer = _mapper.Map<GetCustomerDto>(customerFound);
        //            return Ok(mappedCustomer);
        //        }
        //        else
        //        {
        //            return NotFound("customer not found");
        //        }
        //    }

        //}
        //[HttpPut]
        //[Route("updateCustomer")]
        //[Authorize]
        //public IActionResult UpdateCustomerById([FromBody] UpdateCustomerDto updateCustomer)
        //{
        //    var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var customerToUpdate = _service.GetCustomerById(customerId);
        //    if (customerToUpdate == null)
        //    {
        //        return NotFound();
        //    }
        //    _mapper.Map(updateCustomer, customerToUpdate);
        //    _service.UpdateCustomerDetails(customerToUpdate);

        //    return NoContent();
        //}
        //[HttpDelete]
        //[Route("deleteCustomer")]
        //public IActionResult DeleteCustomerById([FromQuery] string customerId)
        //{
        //    var customerToDelete = _service.GetCustomerById(customerId);
        //    if (customerToDelete == null)
        //    {
        //        return NotFound();
        //    }
        //    _service.DeleteCustomer(customerToDelete);
        //    return NoContent();
        //}

    }
}
