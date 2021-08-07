using Aduaba.Data.DbContexts;
using Aduaba.Data.Models;
using Aduaba.Interfaces;
using Aduaba.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Aduaba.Test
{
    public class UnitTest1
    {
        private readonly ApplicationDbContext _context;
        public UnitTest1()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: "AduabaDbInMemory");
            var dbContextOptions = builder.Options;
            _context = new ApplicationDbContext(dbContextOptions);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

        }
        
        [Fact]
        public async Task GetAll_ReturnItems()
        {

            await _context.Products.AddRangeAsync(Enumerable.Range(1, 10).Select(t => new Data.Models.Product
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 500,
                InStock = true,
                ImageUrl = "viewing",
            }));
            await _context.SaveChangesAsync();

            var productService = new ProductService(_context);
            var result = productService.GetAllProducts();

            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(result);
            Assert.Equal(10, model.Count());
        }

        [Fact]
        public async Task AddNewProductAddsToDB()
        {
            await _context.Products.AddRangeAsync(Enumerable.Range(1, 10).Select(t => new Data.Models.Product
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 500,
                InStock = true,
                ImageUrl = "viewing",
            }));
            await _context.SaveChangesAsync();

            var productService = new ProductService(_context);
            productService.AddProduct(new Product
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 500,
                InStock = true,
                ImageUrl = "viewing",
            });

           
        }
    }
}
