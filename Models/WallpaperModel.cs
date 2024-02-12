using Wallpaper.Entities;

namespace Wallpaper.Models
{
    public class WallpaperModel
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Sort { get; set; }
        public int View { get; set; }
        public long FileSize { get; set; }
        public bool Premium { get; set; }
        public bool Status { get; set; } = false;
        public string Thumbnail { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Ratio { get; set; } = string.Empty;
        public bool Trending { get; set; } = true;
        public string Type { get; set; } = string.Empty;
        public DateTime Create_at { get; set; }
        public DateTime Update_at { get; set;}
    }
}
