using Wallpaper.DTO.Category;

namespace Wallpaper.Service.Category
{
    public interface ICategoryService
    {
        Task<CategoryList_DTO> GetCategoryListAsync(string keyword, int? sort, string colName, bool? isAsc, int index, int size);
    }
}
