using Wallpaper.DTO.Category;
using Wallpaper.Repository.Category;

namespace Wallpaper.Service.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<CategoryList_DTO> GetCategoryListAsync(string keyword, int? sort, string colName, bool? isAsc, int index, int size)
        {
            var categories = await _categoryRepository.GetFilteredAndSortedCategoriesAsync(keyword, sort, colName, isAsc, index, size);
            var totalCategories = await _categoryRepository.GetTotalCountAsync(keyword);
            var totalPages = size != 0 ? (totalCategories / size) + (totalCategories % size > 0 ? 1 : 0) : 0;

            bool isAscending = isAsc ?? true;

            var viewModel = new CategoryList_DTO
            {
                Categories = categories,
                Keyword = keyword,
                Sort = sort,
                ColName = colName,
                IsAsc = isAscending,
                Index = index,
                Size = size,
                TotalPages = totalPages
            };

            return viewModel;
        }
    }
}
