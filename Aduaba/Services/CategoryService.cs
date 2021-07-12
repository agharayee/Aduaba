using Aduaba.Data.DbContexts;
using Aduaba.Data.Models;
using Aduaba.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Services
{
    public class CategoryService : ICategory
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            else
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
            }
        }

        public bool CategoryExists(string categoryId)
        {
            if (categoryId == null)  throw new ArgumentNullException(nameof(categoryId));
            else { return _context.Categories.Any(c => c.Id == categoryId); }
        }

        public void DeleteCategory(Category category)
        {
           if(category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.Include(c => c.Products).ToList();
        }

        public Category GetCategoryById(string categoryId)
        {
            if (categoryId == null)
            {
                throw new ArgumentNullException(nameof(categoryId));
            }
            else { return _context.Categories.FirstOrDefault(c => c.Id == categoryId); }
        }

        public void UpdateCategory(Category category)
        {
            _context.SaveChanges();
        }
    }
}
