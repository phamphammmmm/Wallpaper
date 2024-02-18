using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallpaper.Entities
{
    public class Wallpaper : Base
    {
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Required]
        public int Sort { get; set; }
        public int View { get; set; } = 0;
        [Required]
        public long FileSize { get; set; }
        [Required]
        public bool Premium { get; set; }
        [Required]
        public bool Status { get; set; } = false;
        [Required]
        public string Thumbnail { get; set; } = string.Empty;
        [Required]
        public string Filename { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Path { get; set; } = string.Empty;
        [Required]
        public string Ratio { get; set; } = string.Empty;   
        [Required]
        public bool Trending { get; set; } = true;
        [Required]
        public string Type { get; set; } = string.Empty;
        public ICollection<WallpaperTag> WallpaperTags { get; set; } = new HashSet<WallpaperTag>();

    }
}
