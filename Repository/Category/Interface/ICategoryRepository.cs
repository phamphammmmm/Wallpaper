using System.Collections.Generic;
using Wallpaper.DTO.Category;
using Wallpaper.Entities;
using System.Numerics;

namespace Wallpaper.Repository.Category.Interface
{
    public interface ICategoryRepository    
    {
        Task<List<Entities.Category>> GetFilteredAndSortedCategoriesAsync(string keyword, 
                                                                          int? sort, 
                                                                          string colName, 
                                                                          bool? isAsc, 
                                                                          int index, 
                                                                          int size);
        Task<int> GetTotalCountAsync(string keyword);
        Task<Entities.Category> GetCategoryById(int? id);
        Task<bool> Create(Entities.Category category);
        Task<bool> Edit (Entities.Category category);
        Task<bool> Delete (int? id);
    }
}
