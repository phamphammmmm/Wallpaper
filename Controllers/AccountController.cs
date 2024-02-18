using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Wallpaper.Context;
using Wallpaper.DTO.User;

namespace Wallpaper.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHelper _passwordHelper;

        public AccountController(ApplicationDbContext context, PasswordHelper passwordHelper)
        {
            _context = context;
            _passwordHelper = passwordHelper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin_DTO userLogin_DTO)
        {
            if (await Authenticate(userLogin_DTO))
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userLogin_DTO.Email);
                    if (user == null)
                    {
                        ViewData["ValidateMessage"] = "User not found";
                        return View();
                    }

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role) 
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = userLogin_DTO.KeepLoggedIn
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    Console.WriteLine("Authentication successful");
                    return RedirectToAction("Index", "User");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    throw; // Rethrow the exception for debugging purposes
                }
            }

            ViewData["ValidateMessage"] = "Authentication failed";
            return View();
        }

        private async Task<bool> Authenticate(UserLogin_DTO userLogin_DTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userLogin_DTO.Email);

            if (user != null && _passwordHelper.VerifyPassword(userLogin_DTO.Password, user.Password))
            {
                Console.WriteLine("Authentication succeeded");
                return true;
            }

            // Log unsuccessful authentication
            Console.WriteLine("Authentication failed");
            return false;
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
