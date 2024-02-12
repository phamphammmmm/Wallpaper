
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallpaper.Context;
using Wallpaper.DTO.Tag;
using Wallpaper.Entities;
using Wallpaper.Models;

namespace Wallpaper.Controllers
{
    [AllowAnonymous]
    public class TagController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TagController(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _context = context;
            _webHostEnvironment = environment;
        }
        public async Task<IActionResult> Index()
        {
            var tag = await _context.Tags.ToListAsync();
            return View(tag);
        }

        public async Task<IActionResult> Details(int id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(r => r.Id == id);
            return View(tag);
        }

        // GET: TagController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagCreate_DTO tagCreate_DTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var tagItem = new Tag()
                    {
                        Name = tagCreate_DTO.Name,
                        Create_at = tagCreate_DTO.Create_at,
                    };

                    _context.Tags.Add(tagItem);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Lỗi xảy ra khi tạo Tag. Vui lòng thử lại sau.");
                    return View();
                }
            }
            return View(tagCreate_DTO);
        }


        // GET: TagController/Edit/5
        public IActionResult Edit(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }

            var tag = _context.Tags.FirstOrDefault(r => r.Id == id);
            if(tag == null)
            {
                return NotFound(nameof(Tag));
            }

            var tags = new TagModel()
            {
                Id = tag.Id,
                Name = tag.Name,
            };

            return View(tags);
        }

        // POST: TagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TagEdit_DTO tagEdit_DTO)
        {
            if (id == 0)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(tagEdit_DTO);
            }

            try
            {
                var existingTag = await _context.Tags.FindAsync(id);
                if (existingTag == null)
                {
                    return NotFound();
                }

                existingTag.Name = tagEdit_DTO.Name;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View("Error");
            }
        }


        // GET: TagController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
