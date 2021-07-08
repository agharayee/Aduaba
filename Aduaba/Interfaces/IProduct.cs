using Aduaba.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Interfaces
{
    public interface IProduct
    {
        void AddProduct(Product product);
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(string productId);
        bool ProductExists(string productId);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        IEnumerable<Product> GetAllProductsInACataegoryById(string categoryId);
        Task<List<Product>> SearchResult(string searchParam);
        Task<List<Product>> FilterByPrice(decimal? minPrice, decimal MaxPrice = decimal.MaxValue);
        Task<List<Product>> BestSellingProduct();
        Task<List<Product>> FeaturedProducts();

    }
}
