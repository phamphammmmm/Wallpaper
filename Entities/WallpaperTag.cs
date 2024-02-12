namespace Wallpaper.Entities
{
    public class WallpaperTag
    {
        public int WallpaperId { get; set; }
        public Wallpaper Wallpaper { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; } 
    }
}

