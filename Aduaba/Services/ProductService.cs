using Aduaba.Data.DbContexts;
using Aduaba.Data.Models;
using Aduaba.Interfaces;
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

        public IEnumerable<Product> GetAllProducts()
        {
            var products = _context.Products.ToList();
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

        

        public void UpdateProduct(Product product)
        {
            _context.SaveChanges();
        }
    }
}
