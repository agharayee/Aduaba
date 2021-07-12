using Aduaba.Data.Models;
using Aduaba.Dtos;
using Aduaba.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Controllers
{
    [ApiController]
    //[Route("Category")]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategory _service;

        public CategoryController(IMapper mapper, ICategory service)
        {
            _mapper = mapper;
            _service = service;
        }
        [HttpPost]
        [Route("addCategory")]
        [Authorize]
        public IActionResult AddNewCategory([FromBody] AddCategoryDto category)
        {
            if (ModelState.IsValid)
            {
                var categoryItem = _mapper.Map<Category>(category);
                categoryItem.Id = Guid.NewGuid().ToString();
                _service.AddCategory(categoryItem);
                return Created("Product created successfully",
                    new {categoryId = categoryItem.Id ,CategoryName = categoryItem.Name });
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut]
        [Route("updateCategory")]
        public IActionResult UpdateCategoryById([FromQuery] string categoryId, [FromBody] UpdateCategoryDto updateCategory)
        {
            var categoryToUpdate = _service.GetCategoryById(categoryId);
            if (categoryToUpdate == null)
            {
                return NotFound();
            }
            _mapper.Map(updateCategory, categoryToUpdate);
            _service.UpdateCategory(categoryToUpdate);
            return NoContent();
        }

        [HttpGet("categories")]
        public IActionResult GetAllCategory()
        {
            List<GetCategoryDto> getCategories = new List<GetCategoryDto>();
            var categories = _service.GetAllCategories();
            foreach(var item in categories)
            {
                var categoryToReturn = new GetCategoryDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Length = item.Products.Count,
                    ImageUrl = item.ImageUrl
                };
                getCategories.Add(categoryToReturn);
            }
           
            return Ok(getCategories);
        }

        [HttpGet("category")]
        public IActionResult GetCategory(string categoryId)
        {
            if (categoryId == null) return BadRequest();
            else
            {
                var category = _service.GetCategoryById(categoryId);
                if (category == null) return Ok("Category is empty");
                else
                {
                    var categoryToReturn = new GetCategoryDto
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Length = category.Products.Count,
                        ImageUrl = category.ImageUrl
                    };
                    return Ok(categoryToReturn);
                }
            }
        }
        //[HttpDelete]
        //[Route("deleteCategory")]
        //public IActionResult DeleteCategoryById([FromQuery] string categoryId)
        //{
        //    var categoryToDelete = _service.GetCategoryById(categoryId);
        //    if (categoryToDelete == null)
        //    {
        //        return NotFound();
        //    }
        //    _service.DeleteCategory(categoryToDelete);
        //    return NoContent();
        //}
    }
}
