using Microsoft.EntityFrameworkCore;
using Noma.ApplicationL.Common.Repositories;
using Noma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Noma.Infrastructure.Persistence.Repositories.CategoryRepositories
{
    public class SQLiteCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext context;
        public SQLiteCategoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> InsertCategory(Category category)
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            var updatedCategory = context.Categories.Attach(category);
            updatedCategory.State = EntityState.Modified;

            await context.SaveChangesAsync();

            return category;
        }

        public async Task DeleteCategory(int id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category != null)
            { 
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
            }
        }
    }
}
