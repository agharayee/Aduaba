using Aduaba.Data.Models;
using Aduaba.Dtos;
using Aduaba.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Aduaba.Controllers
{
    [ApiController]
    [Route("Account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;
        private readonly IMapper _mapper;
        //string CustomerId { get; set; }

        public AccountController(IAccountService service, IMapper mapper)
        {
            this._service = service;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(RegisterDto model)
        {
            if (model == null) return BadRequest();
            else
            {
                var result = await _service.RegisterAsync(model);
                if(result.ErrorMessage != null)
                {
                    return BadRequest(result.ErrorMessage);
                }              
                else
                return Ok(result);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(LoginDto model)
        {
            if (model == null) return BadRequest();
            var result = await _service.Login(model);
            if (result == null)
            {
                return BadRequest("Invalid Credentials");
            }

            else
            {

                return Ok(result);
              
            }
           
        }
        [HttpPost]
        [Route("update")]
        [Authorize]
        public IActionResult UpdateCustomerById([FromBody] UpdateCustomerDto updateCustomer)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customerToUpdate = _service.GetCustomerById(customerId);
            if (customerToUpdate == null)
            {
                return NotFound();
            }
            _mapper.Map(updateCustomer, customerToUpdate);
            _service.UpdateCustomerDetails(customerToUpdate);

            return NoContent();
        }

        [HttpDelete]
        [Route("deleteCustomer")]
        [Authorize]
        public IActionResult DeleteCustomerById()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customerToDelete = _service.GetCustomerById(customerId);
            if (customerToDelete == null)
            {
                return NotFound();
            }
            _service.DeleteCustomer(customerToDelete);
            return NoContent();
        }

        [HttpPost("forgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody]string email)
        {
            var result = await  _service.ForgetPassword(email);
            if (result.Errors != null) return BadRequest(result.Errors);
            else
            {

                return Ok("Email Sent Successfully");
            }
        }

        //[HttpPost("ResetPassword")]
        //public async Task<IActionResult> ForgetPassword([FromForm]ResetPasswordDto model)
        //{
        //    if(model.Token.Contains(","))
        //    {
        //        var trimedToken = model.Token.Remove(model.Token.Length - 1);
        //        model.Token = trimedToken;
        //    }
        //    var result = await _service.ResetPassword(model);
        //    if (result.ErrorMessage != null) return BadRequest(result.ErrorMessage);
        //    else
        //    {
        //        return Ok(result.Message);
        //    }
        //}

        [HttpPost("PhoneNumberToken")]
        public async Task<IActionResult> PhoneNumberToken(string email)
        {
            var result = await _service.PhoneNumberConfirmation(email);
            return Ok(result);
        }

        [HttpPost("ConfirmToken")]
        public async Task<IActionResult> PhoneNumberConfirmation(string email, string code)
        {
            var response = await _service.PhoneNumberConfirmation(email, code);
            return Ok(response);
        }
        [HttpPost("ResetPasswordByPhoneNumber")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ResetPasswordDto resetPassword)
        {
            var result = await _service.ResetPasswordAsync(resetPassword);
            return Ok(result.Message);
        }
    }
}
