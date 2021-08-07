using Aduaba.Data.DbContexts;
using Aduaba.Data.Models;
using Aduaba.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Aduaba.Test
{
    public class UnitTest1 : IDisposable
    {
        private readonly ApplicationDbContext _context;
        public UnitTest1()
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

            //Seeding 10 data into the db
            _context.Products.AddRange(
                Enumerable.Range(1, 10).Select(t => new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Name ="Iphone 12",
                    LongDescription = "Correct phone"
                }));
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAll_ReturnItems()
        {
            //Arrange
            await _context.Products.AddRangeAsync(Enumerable.Range(1, 10).Select(t => new Data.Models.Product
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 500,
                InStock = true,
                ImageUrl = "viewing",
            }));
            await _context.SaveChangesAsync();


            //Act
            var productService = new ProductService(_context);
            var result = productService.GetAllProducts();


            //Assert
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(result);
            Assert.Equal(20, model.Count());
        }

        [Fact]
        public async Task AddNewProductAddsToDB()
        {

            //Arrange
            await _context.Products.AddRangeAsync(Enumerable.Range(1, 10).Select(t => new Data.Models.Product
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 500,
                InStock = true,
                ImageUrl = "viewing",
            }));
            await _context.SaveChangesAsync();

            //Act
            var productService = new ProductService(_context);
            productService.AddProduct(new Product
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 500,
                InStock = true,
                ImageUrl = "viewing",
            });

            //Assert
            //Assert.
        }


        [Fact]
        public void GetASingleProductById()
        {
            //Arrange
            var productService = new ProductService(_context);
            var id = Guid.NewGuid().ToString();
            var expected = new Product
            {
                Id = id,
                Amount = 500,
                InStock = true,
                ImageUrl = "viewing",
            };
            productService.AddProduct(expected);

            //Act
            var foundProduct = productService.GetProductById(id);

            //Assert
            Assert.Equal(foundProduct.Id, id);
            Assert.Equal(foundProduct.Amount, expected.Amount);
            Assert.Equal(foundProduct.InStock, expected.InStock);
            Assert.Equal(foundProduct.ImageUrl, expected.ImageUrl);

        }
        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public void DeleteProduct()
        {
            //Arrange
            var productService = new ProductService(_context);
            var id = Guid.NewGuid().ToString();
            var expected = new Product
            {
                Id = id,
                Amount = 500,
                InStock = true,
                ImageUrl = "viewing",
            };
            productService.AddProduct(expected);

            //Act
            productService.DeleteProduct(expected);

            //Let's get created product if it does not return a value it means that it has been deleted
            var actual = productService.ProductExists(id);

            Assert.False(actual);
        }
    }
}
