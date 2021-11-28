using Noma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Noma.ApplicationL.Common.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategoryById(int id);
        Task<Category> InsertCategory(Category category);
        Task<Category> UpdateCategory(Category category);
        Task DeleteCategory(int id);
    }
}
