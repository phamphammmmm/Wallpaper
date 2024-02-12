using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wallpaper.Context;
using Wallpaper.DTO.Category;
using Wallpaper.DTO.Wallpepar;
using Wallpaper.Entities;
using Wallpaper.Models;
using Wallpaper.Service;
using Wallpaper.Service.Tag;
using Wallpaper.Service.Thumbnail;

namespace Wallpaper.Controllers
{
    [AllowAnonymous]
    public class WallpaperController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageService _imageService;
        private readonly IThumbnailService _thumbnailService;
        //private readonly IThumbnailSize _thumbnailSize;
        private readonly ITagService _tagService;
        public WallpaperController(ApplicationDbContext context,
                                   IWebHostEnvironment webHostEnvironment,
                                   IImageService imageService,
                                   ITagService tagService
                                   //IThumbnailService thumbnailService
                                   //IThumbnailSize thumbnailSize
                                   )
        {
            _context = context;
            _tagService = tagService;
            _imageService = imageService;
            //_thumbnailSize = thumbnailSize;
            _webHostEnvironment = webHostEnvironment;
            //_thumbnailService = thumbnailService;
        }

        public async Task<IActionResult> Index( )
        {
            var wallpepars = await _context.Wallpapers.ToListAsync();
            return View(wallpepars);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wallpepar = await _context.Wallpapers.FirstOrDefaultAsync(c => c.Id == id);
            if (wallpepar == null)
            {
                return NotFound();
            }

            return View(wallpepar);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            var categorySelectList = new SelectList(categories, "Id", "Name");
            ViewBag.CategoryList = categorySelectList;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WallpaperCreate_DTO wallpaperCreate_DTO)
        {
            string? imagePath = null;
            string? thumbnailPath = null;
            string? fileRatio = null;

            if (ModelState.IsValid)
            {
                if (wallpaperCreate_DTO.WallpaperFile == null)
                {
                    return NotFound("Không tìm thấy ảnh");
                }

                //Get FileName 
                string fileName = _imageService.GetFileName(wallpaperCreate_DTO.WallpaperFile);

                //Get the ext of file
                string fileExtension = _imageService.GetFileExtension(wallpaperCreate_DTO.WallpaperFile);

                // Define allowed image MIME types
                var allowedImageMimeTypes = new string[] { "image/png", "image/jpeg", "image/jpg", "image/gif","video/mp4" };

                // Check if the MIME type of the file is in the allowed list
                if (allowedImageMimeTypes.Contains(fileExtension))
                {
                    //Save image into the wallpapers folder
                    imagePath = await _imageService.SaveImageAsync(wallpaperCreate_DTO.WallpaperFile,
                                                                          "wallpapers");
                    //Create thumbnail from the uploaded wallpaper
                    thumbnailPath = _thumbnailService.CreateThumbnailFromImage(wallpaperCreate_DTO.WallpaperFile,
                                                                         "thumbnails");
                    //Get the ratio of file
                    fileRatio = _imageService.GetRatioOfFile(wallpaperCreate_DTO.WallpaperFile);
                }
                else if (allowedImageMimeTypes.Contains("video/mp4"))
                {
                    imagePath = await _imageService.SaveImageAsync(wallpaperCreate_DTO.WallpaperFile,
                                                                          "videos");
                    thumbnailPath = await _thumbnailService.CreateThumbnailFromVideo(wallpaperCreate_DTO.WallpaperFile,
                                                                         "thumbnails");
                    //Get the ratio of file
                    //fileRatio = _thumbnailSize.GetRatioOfVideo(imagePath);
                }
                else
                {
                    throw new InvalidOperationException("Unsupported file type");
                }

                // Tách chuỗi tags thành mảng các tag
                string[] tags = wallpaperCreate_DTO.Tags.Split(',');

                // Kiểm tra và tạo mới các tag
                int[] tagIds = await _tagService.GetOrCreateTagsAsync(tags);

                //Get the fileSize
                long fileSize = _imageService.GetFileSize(wallpaperCreate_DTO.WallpaperFile);

                var wallpaper = new Entities.Wallpaper()
                {
                    Path = imagePath,
                    Ratio = fileRatio,
                    Filename = fileName,
                    FileSize = fileSize,
                    Type = fileExtension,
                    Thumbnail = thumbnailPath,
                    Sort = wallpaperCreate_DTO.Sort,
                    Status = wallpaperCreate_DTO.Status,
                    Premium = wallpaperCreate_DTO.Premium,
                    Trending = wallpaperCreate_DTO.Trending,
                    Create_at = wallpaperCreate_DTO.Create_at,
                    CategoryId = wallpaperCreate_DTO.CategoryId,
                    Description = wallpaperCreate_DTO.Description,
                };

                // Thêm wallpaper vào context
                _context.Wallpapers.Add(wallpaper);
                await _context.SaveChangesAsync();

                // Tạo các đối tượng WallpaperTag và thêm vào context
                if (tagIds != null)
                {
                    foreach (var tagId in tagIds)
                    {
                        var wallpaperTag = new WallpaperTag()
                        {
                            WallpaperId = wallpaper.Id,
                            TagId = tagId
                        };
                        _context.WallpaperTags.Add(wallpaperTag);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wallpaper = _context.Wallpapers.FirstOrDefault(c => c.Id == id);

            if (wallpaper == null)
            {
                return base.NotFound(nameof(Entities.Wallpaper));
            }
            var Item = new WallpaperModel
            {
                Id = wallpaper.Id,
                Type = wallpaper.Type,
                Sort = wallpaper.Sort,
                Status = wallpaper.Status,
                Ratio = wallpaper.Ratio,
                Trending = wallpaper.Trending,
                Premium = wallpaper.Premium,
                FileSize = wallpaper.FileSize,
                Thumbnail = wallpaper.Thumbnail,
                Filename = wallpaper.Filename,
                Description = wallpaper.Description,
            };

            return View(Item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Sort,Update_at,WallpaperFile")] WallpaperEdit_DTO wallpaperEdit_DTO)
        {
            if (id == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingWallpaper = await _context.Categories.FindAsync(id);

                if (existingWallpaper == null)
                {
                    return NotFound();
                }

                if (wallpaperEdit_DTO.WallpaperFile != null && wallpaperEdit_DTO.WallpaperFile.Length > 0)
                {
                    var existingImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingWallpaper.Preview.TrimStart('/'));
                    if (System.IO.File.Exists(existingImagePath))
                    {
                        Console.WriteLine("Đang xóa ảnh nè");
                        System.IO.File.Delete(existingImagePath);
                    }

                    // Save the new image
                    var CategoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "category");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(wallpaperEdit_DTO.WallpaperFile.FileName);
                    var filePath = Path.Combine(CategoryPath, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await wallpaperEdit_DTO.WallpaperFile.CopyToAsync(fileStream);
                    }
                    Console.WriteLine("Chuẩn bị thêm ảnh nè");
                    existingWallpaper.Preview = "/category/" + uniqueFileName;
                }
                else
                {
                    Console.WriteLine("File không được tìm thấy");
                }

                // Cập nhật thông tin khác của Category
                existingWallpaper.Name = wallpaperEdit_DTO.Filename;
                existingWallpaper.Sort = wallpaperEdit_DTO.Sort;
                existingWallpaper.Update_at = wallpaperEdit_DTO.Update_at;

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");  // Chuyển hướng sau khi chỉnh sửa thành công
            }

            return View();
        }

    }
}
