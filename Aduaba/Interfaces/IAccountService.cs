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
        Task<string> RegisterAsync(RegisterDto model);
        Task<JwtSecurityToken> Login(LoginDto model);
        Task GetCart(string email);
        void UpdateCustomerDetails(Customer customer);
        Customer GetCustomerById(string customerId);
        bool CustomerExists(string customerId);
        void DeleteCustomer(Customer customer);
    }
}
