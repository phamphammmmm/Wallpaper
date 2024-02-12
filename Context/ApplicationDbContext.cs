using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Wallpaper.Entities;

namespace Wallpaper.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Entities.Wallpaper> Wallpapers { get; set; }
        public DbSet<WallpaperTag> WallpaperTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .Property(u => u.Create_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Category>()
                .Property(u => u.Create_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Tag>()
                .Property(u => u.Create_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Entities.Wallpaper>()
                .Property(u => u.Create_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<UserEntity>()
                .Property(u => u.Update_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Category>()
                .Property(u => u.Update_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Tag>()
                  .Property(u => u.Update_at)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Entities.Wallpaper>()
                  .Property(u => u.Update_at)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Entities.Wallpaper>()
                 .HasOne(w => w.Category)
                 .WithMany(c => c.Wallpapers)
                 .HasForeignKey(w => w.CategoryId);


            modelBuilder.Entity<WallpaperTag>()
                .HasKey(wt => new { wt.WallpaperId, wt.TagId });

            modelBuilder.Entity<WallpaperTag>()
                .HasOne(wt => wt.Wallpaper)
                .WithMany(w => w.WallpaperTags)
                .HasForeignKey(wt => wt.WallpaperId);

            modelBuilder.Entity<WallpaperTag>()
                .HasOne(wt => wt.Tag)
                .WithMany(t => t.WallpaperTags)
                .HasForeignKey(wt => wt.TagId);
        }
    }
}
