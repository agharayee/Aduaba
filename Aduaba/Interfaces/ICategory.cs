using Aduaba.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Interfaces
{
    public interface ICategory
    {
        void AddCategory(Category category);
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(string categoryId);
        bool CategoryExists(string categoryId);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
    }
}
