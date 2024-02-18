using Microsoft.AspNetCore.Mvc;
using Wallpaper.Context;

namespace Wallpaper.Controllers
{
    public class ImportController : Controller
    {
       public async Task<IActionResult> Index()
       {
            return View();
       }

       public async Task<IActionResult> ImportMultipleFile(IFormFile file)
        {
            return View(file);
        }
    }
}
