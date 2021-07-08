using Aduaba.Data.Models;
using Aduaba.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Interfaces
{
    public interface IProduct
    {
       
        void AddProduct(Product product);
        IEnumerable<Product> GetAllProducts(PaginationFilter filter);
        Product GetProductById(string productId);
        bool ProductExists(string productId);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        int GetTotalProducts();
        IEnumerable<Product> GetAllProductsInACataegoryById(string categoryId);
        
    }
}
