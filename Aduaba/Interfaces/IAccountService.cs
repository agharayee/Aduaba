using Aduaba.Data.Models;
using Aduaba.Dtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Interfaces
{
    public interface IAccountService
    {
        Task<ForgetPasswordDto> ForgetPassword(string email);
        Task<RegistrationDto> RegisterAsync(RegisterDto model);
        Task<LoginSucessfulDto> Login(LoginDto model);
        Task GetCart(string email);
        void UpdateCustomerDetails(Customer customer);
        Customer GetCustomerById(string customerId);
        bool CustomerExists(string customerId);
        void DeleteCustomer(Customer customer);
        Task<ResetPasswordReturnDto> ResetPassword(ResetPasswordDto resetPassword);
    }
}
