using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Security.Claims;
using Wallpaper.Context;
using Wallpaper.DTO.User;

namespace Wallpaper.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHelper _passwordHelper;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(ApplicationDbContext context, PasswordHelper passwordHelper, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
            _passwordHelper = passwordHelper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin_DTO userLogin_DTO)
        {
            if (await Authenticate(userLogin_DTO))
            {
                try { 
                    List<Claim> claims = new List<Claim>() 
                    {
                        new Claim(ClaimTypes.NameIdentifier, userLogin_DTO.Email),
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = userLogin_DTO.KeepLoggedIn
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                        Console.WriteLine("Vào tới đây chưa - Authentication successful");
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception: {ex.Message}");
                        throw; // Rethrow the exception for debugging purposes
                    }

            }

            ViewData["ValidateMessage"] = "user not found";
            return View();
        }

        [HttpPost]
        private async Task<bool> Authenticate(UserLogin_DTO userLogin_DTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userLogin_DTO.Email);

            if (user != null)
            {
                // Verify password
                if (_passwordHelper.VerifyPassword(userLogin_DTO.Password, user.Password))
                {
                    // Log successful authentication
                    Console.WriteLine("Authentication succeeded");
                    return true;
                }
            }

            // Log unsuccessful authentication
            Console.WriteLine("Authentication failed");
            return false;
        }

    }
}
