using Microsoft.EntityFrameworkCore;
using System;
using Khdamatk.Server.Contracts.Dashboard;

namespace Khdamatk.Server.Services.Implementations;

    public class CategoryService(Database db) : ICategoryManagementService
    {
        private readonly Database _db = db;

        

        public async Task<List<Category>> GetAllAsync()
        {
            return await _db.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _db.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _db.Categories.AddAsync(category);

            await _db.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> UpdateAsync(int id, Category category)
        {
            var existingCategory =
                await _db.Categories.FindAsync(id);

            if (existingCategory == null)
                return null;

            existingCategory.Name = category.Name;
            existingCategory.Icon = category.Icon;
            existingCategory.IsActive = category.IsActive;
            

            await _db.SaveChangesAsync();

            return existingCategory;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category =
                await _db.Categories.FindAsync(id);

            if (category == null)
                return false;

            _db.Categories.Remove(category);

            await _db.SaveChangesAsync();

            return true;
        }
    }
