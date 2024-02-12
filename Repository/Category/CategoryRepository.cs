using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Wallpaper.Context;
using Wallpaper.DTO.Category;
using Wallpaper.Repository.Category.Interface;

namespace Wallpaper.Repository.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Entities.Category>> GetFilteredAndSortedCategoriesAsync(string keyword, 
                                                                                       int? sort, 
                                                                                       string colName, 
                                                                                       bool? isAsc, 
                                                                                       int index, 
                                                                                       int size)
        {
            var query = _context.Categories.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(r => r.Name.ToLower().Contains(keyword.ToLower()));
            }

            if (sort != null)
            {
                query = query.Where(r => r.Sort == sort);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(colName) && isAsc.HasValue)
            {
                query = colName switch
                {
                    "Id" => isAsc == true ? query.OrderBy(c => c.Id) : query.OrderByDescending(c => c.Id),
                    "Name" => isAsc == true ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name),
                    "Sort" => isAsc == true ? query.OrderBy(c => c.Sort) : query.OrderByDescending(c => c.Sort),
                    "Create_at" => isAsc == true ? query.OrderBy(c => c.Create_at) : query.OrderByDescending(c => c.Create_at),
                    _ => query,
                };
            }

            // Apply pagination
            var categories = await query.Skip((index - 1) * size)
                                        .Take(size)
                                        .ToListAsync();

            return categories;
        }

        public async Task<int> GetTotalCountAsync(string keyword)
        {
            var total = await _context.Categories.CountAsync();
            return total;
        }

        public async Task<Entities.Category?> GetCategoryById(int? id)
        {
            if(id == null)
            {
                return null;
            }

            var category =  await _context.Categories.FirstOrDefaultAsync(r => r.Id == id);
            if(category == null)
            {
                return null;
            }

            return category;
        }

        public async Task<bool>Create(Entities.Category category)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException )
            {
                return false;
            }
        }

        public async Task<bool>Delete(int? id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            return true;
        }

        public async Task<bool>Edit(Entities.Category category)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }
    }
}
