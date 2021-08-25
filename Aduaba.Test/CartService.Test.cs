using Aduaba.Data.DbContexts;
using Aduaba.Data.Models;
using Aduaba.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Aduaba.Test
{
    public class CartServiceTest
    {
        private readonly ApplicationDbContext _context;
        private readonly CartService _sut;



        public CartServiceTest()
        {
            //By supplying new service provider for every context, we have a single database instance per test
            var serviceProvider = new ServiceCollection().
                AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            //Build context options
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>().
                UseInMemoryDatabase(databaseName: "AduabaInMemoryDb")
                .UseInternalServiceProvider(serviceProvider);

            _context = new ApplicationDbContext(builder.Options);

            _sut = new CartService(_context);
        }
        [Fact]
        public async Task AddToCart()
        {
            // Arrange Make a user
            var newUser = new Customer
            {
                Id = "1",
                Password = "Lookup1234#",
                Email = "agharayee@gmail.com",
                UserName = "Boboyor",
                FirstName = "Joshua",
                LastName = "Governor"
            };

            var result = await CreateUser(newUser);
            var id = Guid.NewGuid().ToString();
            //Make products
            var product = new Product
            {
                Id = id,
                Amount = 5000M,
                InStock = true,
                Name = "Play Station 5",
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            //Act
            //call the add to cart method
            _sut.AddToCart(id, newUser.Id);
            var customerCart = _sut.GetCart(newUser.Id);

            //Assert
            //Check his current cart to know if the product was added
            Assert.Single(customerCart);
            Assert.Equal(product.Name, customerCart[0].Product.Name);
            Assert.Equal(product.Amount, customerCart[0].Product.Amount);

        }

        [Fact]
        public async Task UpdateCartQuantity()
        {
            // Arrange Make a user
            var newUser = new Customer
            {
                Id = "1",
                Password = "Lookup1234#",
                Email = "agharayee@gmail.com",
                UserName = "Boboyor",
                FirstName = "Joshua",
                LastName = "Governor"
            };

            var result = await CreateUser(newUser);
            var id = Guid.NewGuid().ToString();
            //Make products
            var product = new Product
            {
                Id = id,
                Amount = 5000M,
                InStock = true,
                Name = "Play Station 5",
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            //Act
            //call the add to cart method
            _sut.AddToCart(id, newUser.Id);
            _sut.AddToCart(id, newUser.Id);
            var customerCart = _sut.GetCart(newUser.Id);

            //Assert
            //Check his current cart to know if the product was added
            Assert.Single(customerCart);
            Assert.Equal(product.Name, customerCart[0].Product.Name);
            Assert.Equal(product.Amount, customerCart[0].Product.Amount);
            Assert.Equal(2, customerCart[0].Quantity);

        }


        List<Customer> customers = new List<Customer>();
        public UserManager<Customer> CreateUsers()
        {
           
            var newCustomer1 = new Customer
            {
                Id = "123",
                FirstName = "Emmanuel",
                LastName = "Agharaye",
                Email = "agharayee@gmail.com",
                Password = "Lookup1234#",
            };

            var newCustomer2 = new Customer
            {
                Id = "1234",
                FirstName = "Boboyor",
                LastName = "Tseyi",
                Email = "agharayee@gmail.com",
                Password = "Lookup1234#",
            };

            customers.Add(newCustomer2);
            customers.Add(newCustomer1);

            var accountOpener = MockUserManager<Customer>(customers).Object;
            return accountOpener;
        }


        public async Task<string> CreateUser(Customer user)
        {
            var users = CreateUsers();
            var result = await users.CreateAsync(user, user.Password);
            if (result.Succeeded) return "Successful";
            else return "Not Successful";
        }

        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }

        [Fact]
        public async Task CreateAUser()
        {
            var newUser = new Customer
            {
                Password = "Lookup1234#",
                Email = "agharayee@gmail.com",
                UserName = "Boboyor",
                FirstName = "Joshua",
                LastName = "Governor"
            };

            var result = await CreateUser(newUser);


            Assert.Equal(3, customers.Count);
        }
    }
}
