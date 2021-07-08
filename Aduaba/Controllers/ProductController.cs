using Aduaba.Data.Models;
using Aduaba.Dtos;
using Aduaba.Interfaces;
using Aduaba.RequestFeatures;
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
        private readonly IUriService _uriService;

        public ProductController(IProduct service, IMapper mapper, ICategory category, IUriService uriService)
        {
            _service = service;
            _mapper = mapper;
            _category = category;
            _uriService = uriService;
        }
        [HttpPost]
        [Route("addProducts")]
        public IActionResult AddNewProduct([FromBody]AddProductDto product)
        {
            if (ModelState.IsValid) 
            {
                var productIem = _mapper.Map<Product>(product);
                productIem.Id = Guid.NewGuid().ToString();
                _service.AddProduct(productIem);
                return Created("Product created successfully",
              new { ProductName = productIem.Name, ProductAmount = productIem.Amount, Description=productIem.ShortDescription, ProductId = productIem.Id } );
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("getProducts")]
        [Authorize]
        public IActionResult GetAllProducts([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = _service.GetAllProducts(validFilter);
            var totalProducts = _service.GetTotalProducts();
            var mappedProducts = _mapper.Map<IEnumerable<GetProductDto>>(products);
            if (!mappedProducts.Any())
            {
                return Ok("No product found");
            }
            else
            {
                return Ok(mappedProducts);

            }
            
            

        }
        [HttpGet]
        [Route("productId")]
        public IActionResult GetProductById([FromQuery]string productId)
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
        public IActionResult UpdateProductById([FromQuery]string productId, [FromBody]UpdateProductDto updateProduct)
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
        [HttpDelete]
        [Route("deleteProducts")]
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
