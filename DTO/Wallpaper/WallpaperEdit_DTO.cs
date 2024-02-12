using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallpaper.DTO.Wallpepar
{
    public class WallpaperEdit_DTO
    {
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int Sort { get; set; }
        [Required]
        public int View { get; set; }
        [Required]
        public int FileSize { get; set; }
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
        public DateTime Update_at { get; set; }

        [NotMapped]
        public IFormFile? WallpaperFile { get; set; }
    }
}
