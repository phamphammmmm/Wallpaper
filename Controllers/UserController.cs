using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Wallpaper.Context;
using Wallpaper.DTO.User;
using Wallpaper.Entities;
using Wallpaper.Models;

namespace Wallpaper.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHelper _passwordHelper;

        public UserController(ApplicationDbContext context, PasswordHelper passwordHelper)
        {
            _context = context;
            _passwordHelper = passwordHelper;
        }

        public async Task<IActionResult> Index()
        {
           var user = await _context.Users.ToListAsync();
            return View(user);
        }

        // GET: UserEntities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userEntity = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userEntity == null)
            {
                return NotFound();
            }

            return View(userEntity);
        }

        // GET: UserEntities/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Password,Email,Create_at")] UserCreate_DTO userCreate_DTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newUser = new UserEntity
                    {
                        Name = userCreate_DTO.Name,
                        Password = _passwordHelper.HashPassword(userCreate_DTO.Password),
                        Email = userCreate_DTO.Email,
                        Create_at = userCreate_DTO.Create_at
                    };

                    _context.Add(newUser);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during user creation: {ex.Message}");
                }
            }

            var userEntity = await _context.Users.FindAsync(userCreate_DTO.Email);

            if (userEntity == null)
            {
                return NotFound();
            }

            var userModel = new UserModel
            {
                Name = userEntity.Name,
                Password = userEntity.Password,
                Email = userEntity.Email,
                Role = userEntity.Role,
                Create_at = userEntity.Create_at,
                Update_at = userEntity.Update_at
            };

            return View(userCreate_DTO);
        }


        // GET: UserEntities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userEntity = await _context.Users.FindAsync(id);
            if (userEntity == null)
            {
                return NotFound();
            }
            return View(userEntity);
        }

        // POST: UserEntities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Password,Email,Role,Update_at")] UserEdit_DTO editedUser)
        {
            if (id != editedUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.FindAsync(id);

                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    existingUser.Name = editedUser.Name;
                    existingUser.Password = editedUser.Password;
                    existingUser.Email = editedUser.Email;
                    existingUser.Role = editedUser.Role;
                    existingUser.Update_at = editedUser.Update_at;

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserEntityExists(editedUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var userEntity = await _context.Users.FindAsync(id);

            if (userEntity == null)
            {
                return NotFound();
            }

            var userModel = new UserModel
            {
                Name = userEntity.Name,
                Password = userEntity.Password,
                Email = userEntity.Email,
                Role = userEntity.Role,
                Create_at = userEntity.Create_at,
                Update_at = userEntity.Update_at
            };

            return View(userModel);
        }

        // GET: UserEntities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userEntity = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userEntity == null)
            {
                return NotFound();
            }

            return View(userEntity);
        }

        // POST: UserEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userEntity = await _context.Users.FindAsync(id);
            if (userEntity != null)
            {
                _context.Users.Remove(userEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserEntityExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
