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
    public class ProductTest : IDisposable
    {
        private readonly ApplicationDbContext _context;
        public ProductTest()
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
                    Name = "Iphone 12",
                    LongDescription = "Correct phone"
                }));
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllProducts()
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
        public void AddNewProductToDB()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var expected = new Product
            {
                Id = id,
                Amount = 500,
                InStock = true,
                ImageUrl = "viewing",
            };
            var createdProduct = new ProductService(_context);
            createdProduct.AddProduct(expected);

            //Act
            var actual = createdProduct.ProductExists(id);

            //Assert
            Assert.True(actual);
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
            //Add a product to the db
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

            //Then delete the added product
            productService.DeleteProduct(expected);

            //check if the added product exist
            var actual = productService.ProductExists(id);

            Assert.False(actual);
        }

        [Fact]
        public async Task SearchResult()
        {
            //Act 
            await _context.Products.AddRangeAsync(Enumerable.Range(1, 5).Select(t => new Data.Models.Product
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 500,
                InStock = true,
                Name = "Play Station 5",
                LongDescription = "This is a ps 5 that can play on any TV",
                ImageUrl = "viewing",
            }));
            await _context.SaveChangesAsync();

            //Act
            var productService = new ProductService(_context);
            var result = await productService.SearchResult("ps 5");

            //Assert
            var model = Assert.IsAssignableFrom<List<Product>>(result);
            Assert.Equal(5, model.Count());
        }

        [Fact]
        public async Task SearchResultWithoutProduct()
        {
            //Act 
            await _context.Products.AddRangeAsync(Enumerable.Range(1, 5).Select(t => new Data.Models.Product
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 500,
                InStock = true,
                Name = "Play Station 5",
                LongDescription = "This is a ps 5 that can play on any TV",
                ImageUrl = "viewing",
            }));
            await _context.SaveChangesAsync();

            //Act
            var productService = new ProductService(_context);
            var result = await productService.SearchResult("gdhdhdhdhdhhdajs");

            //Assert
            var model = Assert.IsAssignableFrom<List<Product>>(result);
            Assert.Empty(model);
        }

        [Fact]
        public async Task FilterByPriceWithRightPrice()
        {
            //Act 
            await _context.Products.AddRangeAsync(Enumerable.Range(1, 5).Select(t => new Data.Models.Product
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 500,
                InStock = true,
                Name = "Play Station 5",
                LongDescription = "This is a ps 5 that can play on any TV",
                ImageUrl = "viewing",

            }));
            await _context.SaveChangesAsync();

            //Act
            var productService = new ProductService(_context);
            var result = await productService.FilterByPrice(300, 600);

            //Assert
            var model = Assert.IsAssignableFrom<List<Product>>(result);
            Assert.Equal(5, model.Count());
        }

        [Fact]
        public async Task FilterByPriceWithWrongPrice()
        {
            //Act 
            await _context.Products.AddRangeAsync(Enumerable.Range(1, 5).Select(t => new Data.Models.Product
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 500,
                InStock = true,
                Name = "Play Station 5",
                LongDescription = "This is a ps 5 that can play on any TV",
                ImageUrl = "viewing",

            }));
            await _context.SaveChangesAsync();

            //Act
            var productService = new ProductService(_context);
            var result = await productService.FilterByPrice(700, 600);

            //Assert
            var model = Assert.IsAssignableFrom<List<Product>>(result);
            Assert.Empty(model);
        }

        [Fact]
        public async Task ProductNotFeaturedProudct()
        {
            //Act 
            await _context.Products.AddRangeAsync(Enumerable.Range(1, 5).Select(t => new Data.Models.Product
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 500,
                InStock = true,
                Name = "Play Station 5",
                LongDescription = "This is a ps 5 that can play on any TV",
                ImageUrl = "viewing",

            }));
            await _context.SaveChangesAsync();

            //Act
            var productService = new ProductService(_context);
            var result = await productService.FeaturedProducts();

            var model = Assert.IsAssignableFrom<List<Product>>(result);
            Assert.Empty(model);
        }

        [Fact]
        public async Task ProductAFeaturedProudct()
        {
            //Act 
            await _context.Products.AddRangeAsync(Enumerable.Range(1, 5).Select(t => new Data.Models.Product
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 500,
                InStock = true,
                Name = "Play Station 5",
                LongDescription = "This is a ps 5 that can play on any TV",
                ImageUrl = "viewing",
                IsFeaturedProduct = true
            }));
            await _context.SaveChangesAsync();

            //Act
            var productService = new ProductService(_context);
            var result = await productService.FeaturedProducts();

            var model = Assert.IsAssignableFrom<List<Product>>(result);
            Assert.Equal(5, model.Count);
        }


    }
}
