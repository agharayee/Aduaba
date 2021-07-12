using Aduaba.Data.Models;
using Aduaba.Dtos;
using Aduaba.Interfaces;
using Aduaba.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Aduaba.Controllers
{
    [ApiController]
   // [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _service;
        private readonly IMapper _mapper;
        private readonly ICategory _category;

        public ProductController(IProduct service, IMapper mapper, ICategory category)
        {
            _service = service;
            _mapper = mapper;
            _category = category;
        }
        [HttpPost]
        [Route("addProducts")]
        [Authorize]
        public IActionResult AddNewProduct([FromBody] AddProductDto product)
        {
            if (ModelState.IsValid)
            {
                var productIem = _mapper.Map<Product>(product);
                productIem.Id = Guid.NewGuid().ToString();
                _service.AddProduct(productIem);
                return Created("Product created successfully",
              new { ProductName = productIem.Name, ProductAmount = productIem.Amount, Description = productIem.ShortDescription, ProductId = productIem.Id });
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("getProducts")]
        public IActionResult GetAllProducts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = _service.GetAllProducts();
            
            if (!products.Any())
            {
                return Ok("No product found");
            }
            else
            {
                List<GetProductDto> myString = new List<GetProductDto>(); ;
                string isAvailable = default;
                foreach (var item in products)
                {
                    if (item.InStock == true) isAvailable = "InStock";
                    else isAvailable = "Out of Stock";
                    var getAllProduct = new GetProductDto
                    {
                        FeaturedProduct = item.IsFeaturedProduct,
                        Amount = item.Amount,
                        IsAvailable = isAvailable,
                        ShortDescription = item.ShortDescription,
                        BestSelling = item.IsBestSelling,
                        LongDescription = item.LongDescription,
                        InStock = item.InStock,
                        CategoryId = item.CategoryId,
                        ImageUrl = item.ImageUrl,
                        Manufacturer = item.Manufacturer,
                        Name = item.Name,
                        Quantity = item.Quantity
                    };
                    myString.Add(getAllProduct);
                }
                return Ok(myString);
            }
        }
        [HttpGet]
        [Route("productId")]
        public IActionResult GetProductById([FromQuery] string productId)
        {
            if (productId == null)
            {
                return BadRequest();
            }
            else
            {
                if (_service.ProductExists(productId))
                {
                    var productFound = _service.GetProductById(productId);
                    var mappedProduct = _mapper.Map<GetProductDto>(productFound);
                    return Ok(mappedProduct);
                }
                else
                {
                    return NotFound("product not found");
                }
            }

        }
        [HttpGet]
        [Route("categoryId")]
        public IActionResult GetProductByCategoryId(string categoryId)
        {
            if (categoryId == null)
            {
                return BadRequest();
            }
            else
            {
                if (_category.CategoryExists(categoryId))
                {
                    var productFound = _service.GetAllProductsInACataegoryById(categoryId);
                    var mappedProduct = _mapper.Map<IEnumerable<GetProductDto>>(productFound);
                    return Ok(mappedProduct);
                }
                else
                {
                    return NotFound("product not found");
                }
            }

        }
        [HttpPut]
        [Route("updateProduct")]
        [Authorize]
        public IActionResult UpdateProductById([FromQuery] string productId, [FromBody] UpdateProductDto updateProduct)
        {
            var productToUpdate = _service.GetProductById(productId);
            if (productToUpdate == null)
            {
                return NotFound();
            }
            _mapper.Map(updateProduct, productToUpdate);
            _service.UpdateProduct(productToUpdate);

            return NoContent();
        }

        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> SearchProduct(string searchParam)
        {
            if (searchParam == null)
            {
                var products = _service.GetAllProducts();
                var mappedProducts = _mapper.Map<IEnumerable<GetProductDto>>(products);
                return Ok(mappedProducts);
            }
            else
            {
                var products = await _service.SearchResult(searchParam);
                if(products.Count == 0)
                {
                    return Ok("No product found");
                }else
                {
                    var mappedProducts = _mapper.Map<IEnumerable<GetProductDto>>(products);
                    return Ok(mappedProducts);
                }
               
            }
        }


        [HttpGet]
        [Route("FilterProduct")]
        public async Task<IActionResult> FilterProduct(decimal? minValue, decimal maxValue = decimal.MaxValue)
        {
            if (minValue == null)
            {
                var products = _service.GetAllProducts();
                var mappedProducts = _mapper.Map<IEnumerable<GetProductDto>>(products);
                return Ok(mappedProducts);
            }
            else
            {
                var filterProducts = await _service.FilterByPrice(minValue, maxValue);
                var mappedProducts = _mapper.Map<IEnumerable<GetProductDto>>(filterProducts);
                return Ok(mappedProducts);
            }
        }

        [HttpGet]
        [Route("BestSellingProduct")]
        public async Task<IActionResult> BestSellingProduct()
        {
            var bestSellingProducts = await _service.BestSellingProduct();
            if (bestSellingProducts == null) return Ok("No product Found");
            else
            {
                var mappedBestSellingProducts = _mapper.Map<IEnumerable<GetProductDto>>(bestSellingProducts);
                return Ok(mappedBestSellingProducts);
            }
        }

        [HttpGet]
        [Route("FeaturedProduct")]
        public async Task<IActionResult> FeaturedProduct()
        {
            var featuredProducts = await _service.FeaturedProducts();
            if (featuredProducts == null) return Ok("No product Found");
            else
            {
                var mappedfeaturedProducts = _mapper.Map<IEnumerable<GetProductDto>>(featuredProducts);
                return Ok(mappedfeaturedProducts);
            }
        }


        [HttpDelete]
        [Route("deleteProducts")]
        [Authorize]
        public IActionResult DeleteProductById([FromQuery] string productId)
        {
            var productToDelete = _service.GetProductById(productId);
            if (productToDelete == null)
            {
                return NotFound();
            }
            _service.DeleteProduct(productToDelete);
            return NoContent();
        }
    }
}
    

