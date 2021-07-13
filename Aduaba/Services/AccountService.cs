using Aduaba.Authorizations;
using Aduaba.Data.DbContexts;
using Aduaba.Data.Models;
using Aduaba.Dtos;
using Aduaba.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
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
        private IEmailSender _emailSender;
        private ITextMessageService _textMessageService;
        string CustomerId { get; set; }
        public AccountService(UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager, 
            Microsoft.Extensions.Configuration.IConfiguration configuration, ApplicationDbContext context, ICartService cartService, IWishListService wishList,
                    IEmailSender emailSender, ITextMessageService textMessageService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _wishList = wishList;
            _cartService = cartService;
            _emailSender = emailSender;
            _textMessageService = textMessageService;
        }

        public async Task<LoginSucessfulDto> Login(LoginDto model)
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

            var loginDto = new LoginSucessfulDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo.ToString("yyyy-MM-ddThh:mm:ss"),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
                };
            return loginDto;
            
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


        public async Task<RegistrationDto> RegisterAsync(RegisterDto model)
        {
            RegistrationDto returnDto = default; 
            var user = new Customer
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageUrl = model.ImageUrl,
                PhoneNumber = model.PhoneNumber
            };
            returnDto = new RegistrationDto
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber
                
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Aduaba.Authorizations.Authorization.default_role.ToString());
                    return returnDto;
                }
                else
                {
                    var errors = AddErrors(result);
                    returnDto.ErrorMessage = errors;
                    return returnDto;
                }
                
            }
            else
            {
                var error =  $"Email {user.Email } is already registered.";
                returnDto.ErrorMessage = error;
                return returnDto;
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

        public async Task<ForgetPasswordDto> ForgetPassword(string email)
        {
            ForgetPasswordDto forgetPassword = default;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                forgetPassword = new ForgetPasswordDto
                {
                    Errors = "No User Found with this email",
                };
            return forgetPassword;
            }
            else
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = Encoding.UTF8.GetBytes(token);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                string returnUrl = $"https://teamaduaba.azurewebsites.net/ResetPassword?email={email}&token={validToken}";
                //string developmentReturnUrl = $"https://localhost:5001/ResetPassword?email={email}&token={validToken}";
                //var body = "This is a test message";

                _emailSender.SendEmailAsync(email, "Password Reset", "<h3>Password Reset, Please follow the instrutions to reset your password</h3>" + $"<p>To reset your password<a " +
                                             $"href='{ returnUrl  },'> Click here to reset your password</a></p>");
               // _textMessageService.SendMessage(user.PhoneNumber, body);
                forgetPassword = new ForgetPasswordDto
                {
                    Token = validToken,
                    RedirectUri = returnUrl,
                    
                };
                return forgetPassword;
            }
        }

        public async Task<ResetPasswordReturnDto> ResetPassword(ResetPasswordDto resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
                return new ResetPasswordReturnDto
                {
                    IsSuccessful = false,
                    Message = "No user associated with email",
                };

            if (resetPassword.NewPassword != resetPassword.ComfirmPassword)
                return new ResetPasswordReturnDto
                {
                    IsSuccessful = false,
                    Message = "Password doesn't match its confirmation",
                };

            var decodedToken = WebEncoders.Base64UrlDecode(resetPassword.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, resetPassword.NewPassword);

            if (result.Succeeded)
                return new ResetPasswordReturnDto
                {
                    Message = "Password has been reset successfully!",
                    IsSuccessful = true,
                };
            
            return new ResetPasswordReturnDto
            {
                Message = "Something went wrong",
                IsSuccessful = false,
                ErrorMessage = AddErrors(result),
            };
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

        private string AddErrors(IdentityResult result)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var error in result.Errors)
            {
                sb.Append(error.Description + " Registration Failed. ");
            }
            return sb.ToString();
        }


        private static readonly Random r = new Random(DateTime.Now.Second);
        public int Otp => r.Next(1, 19);

    }
}
