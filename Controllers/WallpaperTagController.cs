using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wallpaper.Context;
using Wallpaper.Entities;

namespace Wallpaper.Controllers
{
    public class WallpaperTagController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationDbContext _context;

        public WallpaperTagController(IHttpContextAccessor contextAccessor, ApplicationDbContext context)
        {
            _contextAccessor = contextAccessor;
            _context = context;
        }

        // GET: WallpaperTagController
        public ActionResult Index()
        {
            return View();
        }

        // GET: WallpaperTagController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WallpaperTagController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WallpaperTagController/Create
        public async Task<IActionResult> Create(WallpaperTag model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.WallpaperTags.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the wallpaper tag.");
                    return View(model);
                }
            }
            return View(model);
        }

        // GET: WallpaperTagController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WallpaperTagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: WallpaperTagController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WallpaperTagController/Delete/5
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
