using Aduaba.Authorizations;
using Aduaba.Data.DbContexts;
using Aduaba.Data.Models;
using Aduaba.Dtos;
using Aduaba.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Aduaba.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<Customer> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private ApplicationDbContext _context;
        private ICartService _cartService;
        private IWishListService _wishList;
        string CustomerId { get; set; }
        public AccountService(UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager, 
            Microsoft.Extensions.Configuration.IConfiguration configuration, ApplicationDbContext context, ICartService cartService, IWishListService wishList)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _wishList = wishList;
            _cartService = cartService;
        }

        public async Task<JwtSecurityToken> Login(LoginDto model)
        {
            JwtSecurityToken token = default;
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSiginKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
                 token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(2),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSiginKey, SecurityAlgorithms.HmacSha256Signature)
                    ); ;
            }
            else
            {
                return null;
            }
            
            return token;
            
        }
        
        public async Task GetCart(string email)
        {
            string productId = default;
            int Quantity = default;
            var Customer = await _userManager.FindByEmailAsync(email);
            var customerId = Customer.Id;

            var cartItems = _cartService.GetShoppingCartItems();
            foreach (var item in cartItems)
            {
                productId = item.ProductId;
                Quantity = item.Quantity;
                _cartService.AddToCart(productId, Quantity, customerId);
                _cartService.RemoveFromCartWithSession(productId);
            }
        }

        public async Task GetWishList(string email)
        {
            string productId = default;
            var Customer = await _userManager.FindByEmailAsync(email);
            var customerId = Customer.Id;

            var wishListItem = _wishList.GetCustomerWishListItems(customerId);
            foreach (var item in wishListItem)
            {
                productId = item.ProductId;
                var wishList = new WishList
                {
                    CustomerId = customerId
                };
                await _wishList.AddToWishList(wishList, productId);
            }
        }


        public async Task<string> RegisterAsync(RegisterDto model)
        {
            var user = new Customer
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Aduaba.Authorizations.Authorization.default_role.ToString());
                    return $"User Registered with username {user.UserName}";
                }
                else
                {
                    return $"Password must contain uppercase, lowercase, special character, number and be eight digits long";
                }
                
            }
            else
            {
                return $"Email {user.Email } is already registered.";
            }
        }


        public void UpdateCustomerDetails(Customer customer)
        {
            _context.SaveChanges();
        }

        public Customer GetCustomerById(string customerId)
        {
            if (customerId == null) { throw new ArgumentNullException(nameof(customerId)); }
            else
            {
                var customer = _context.Users.FirstOrDefault(p => p.Id == customerId);
                return customer;
            }
        }

        public bool CustomerExists(string customerId)
        {
            if (customerId == null) { throw new ArgumentNullException(nameof(customerId)); }
            else return _context.Users.Any(c => c.Id == customerId);
        }
        public void DeleteCustomer(Customer customer)
        {
            //if (customer == null)
            //{
            //    throw new ArgumentNullException(nameof(customer));
            //}
            //_context.Customers.Remove(customer);
            //_context.SaveChanges();
        }

    }
}
