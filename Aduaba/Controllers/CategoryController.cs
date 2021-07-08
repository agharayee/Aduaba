using Aduaba.Data.Models;
using Aduaba.Dtos;
using Aduaba.Interfaces;
using AutoMapper;
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
