using Aduaba.Data.DbContexts;
using Aduaba.Data.Models;
using Aduaba.Interfaces;
using Aduaba.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Services
{
    public class ProductService : IProduct
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            else
            {
                _context.Products.Add(product);
                _context.SaveChanges();
            }
        }

        public void DeleteProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public IEnumerable<Product> GetAllProducts(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var products = _context.Products.Include(e => e.Category).OrderBy(c => c.Name)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return products;
        }

        public IEnumerable<Product> GetAllProductsInACataegoryById(string categoryId)
        {
            if (categoryId == null) { throw new ArgumentNullException(nameof(categoryId)); }
            else
            {
                var products = _context.Products.Where(p => p.CategoryId == categoryId);
                return products;
            }
        }
       

        public Product GetProductById(string productId)
        {
            if (productId == null) { throw new ArgumentNullException(nameof(productId)); }
            else
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == productId);
                return product;
            }
        }

        public bool ProductExists(string productId)
        {
            if (productId == null) { throw new ArgumentNullException(nameof(productId)); }
            else return _context.Products.Any(p => p.Id == productId);
        }

        public int GetTotalProducts()
        {
            var totalRecords =  _context.Products.Count();
            return totalRecords;
        }

        public void UpdateProduct(Product product)
        {
            _context.SaveChanges();
        }
    }
}
