using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using Wallpaper.Context;
using Wallpaper.DTO.Category;
using Wallpaper.DTO.Wallpepar;
using Wallpaper.Entities;
using Wallpaper.Models;
using Wallpaper.Repository.Category.Interface;
using Wallpaper.Service;
using Wallpaper.Service.Category;
using Wallpaper.Service.Thumbnail;

namespace Wallpaper.Controllers
{
    [AllowAnonymous]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IThumbnailService _thumbnailService;
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imageService;

        public CategoryController(ApplicationDbContext context,
                                  IWebHostEnvironment webHostEnvironment,
                                  ICategoryRepository categoryRepository,
                                  IThumbnailService thumbnailService,
                                  IImageService imageService,
                                  ICategoryService categoryService 
                                  )
        {
            _context = context;
            _categoryService = categoryService;
            _thumbnailService = thumbnailService;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _imageService = imageService;
        }

        public async Task<IActionResult> Index(string keyword = "", int? sort = null,
                                           string colName = "Id", bool? isAsc = true,
                                           int index = 1, int size = 3)
        {
            var viewModel = await _categoryService.GetCategoryListAsync(keyword, sort, colName, isAsc, index, size);
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var category = await _categoryRepository.GetCategoryById(id);   
            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreate_DTO categoryCreate_DTO)
        {
            if (ModelState.IsValid)
            {
                if (categoryCreate_DTO.CategoryFile != null && categoryCreate_DTO.CategoryFile.Length > 0)
                {
                    await _imageService.SaveImageAsync(categoryCreate_DTO.CategoryFile,
                                                                           "category");
                    //Create thumbnail from the uploaded wallpaper
                    string thumbnailPath = _thumbnailService.CreateThumbnailFromImage(categoryCreate_DTO.CategoryFile,
                                                                         "thumbnails");
                    var categories = new Category
                    {
                        Name = categoryCreate_DTO.Name,
                        Sort = categoryCreate_DTO.Sort,
                        Create_at = categoryCreate_DTO.Create_at,
                        Preview = thumbnailPath
                    };

                    await _categoryRepository.Create(categories);
                }
                else
                {
                    Console.WriteLine("File không được tìm thấy");
                }

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _categoryRepository.GetCategoryById(id.Value);

            var Item = new CategoryModel
            {
                Id = category.Id,
                Name = category.Name,
                Sort = category.Sort,
            };

            return View(Item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Sort,Update_at,CategoryFile")] CategoryEdit_DTO categoryEdit_DTO)
        {
            if (id == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingCategory = await _context.Categories.FindAsync(id);

                if (existingCategory == null)
                {
                    return NotFound();
                }

                if (categoryEdit_DTO.CategoryFile != null && categoryEdit_DTO.CategoryFile.Length > 0)
                {
                    var existingImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingCategory.Preview.TrimStart('/'));
                    if (System.IO.File.Exists(existingImagePath))
                    {
                        System.IO.File.Delete(existingImagePath);
                    }

                    // Save the new image
                    var CategoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "category");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(categoryEdit_DTO.CategoryFile.FileName);
                    var filePath = Path.Combine(CategoryPath, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await categoryEdit_DTO.CategoryFile.CopyToAsync(fileStream);
                    }

                    existingCategory.Preview = "/category/" + uniqueFileName;
                }
                else
                {
                    Console.WriteLine("File không được tìm thấy");
                }

                // Cập nhật thông tin khác của Category
                existingCategory.Name = categoryEdit_DTO.Name;
                existingCategory.Sort = categoryEdit_DTO.Sort;
                existingCategory.Update_at = categoryEdit_DTO.Update_at;

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");  // Chuyển hướng sau khi chỉnh sửa thành công
            }

            return View();
        }

        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                await _categoryRepository.Delete(id);
            }

            TempData["SuccessMessage"] = "Category deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
