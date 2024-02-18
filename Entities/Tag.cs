using System.ComponentModel.DataAnnotations;

namespace Wallpaper.Entities
{
    public class Tag : Base
    {
        [Required] 
        public string Name { get; set; } = string.Empty;
        public ICollection<WallpaperTag> WallpaperTags { get; set; } = new List<WallpaperTag>();
    }
}
