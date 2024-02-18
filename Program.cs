using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Wallpaper.Context;
using Wallpaper.Controllers;
using Wallpaper.Repository.Category;
using Wallpaper.Service.Tag;
using Wallpaper.Service.Wallpaper.Inteface;
using Wallpaper.Service.Thumbnail;
using Wallpaper.Service;
using Microsoft.VisualBasic;
using Wallpaper.Service.Category;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext to connect to the database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "Wallpaper.Cookie";
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout"; 
    });

// Add Dependency Injection
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IThumbnailService, ThumbnailService>();
//builder.Services.AddScoped<IThumbnailSize, ThumbnailSize>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<PasswordHelper>();

builder.Services.AddHttpContextAccessor();
//builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCookiePolicy();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
