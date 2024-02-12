using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallpaper.DTO.Wallpepar
{
    public class WallpaperCreate_DTO
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int Sort { get; set; }
        [Required]
        public bool Premium { get; set; }
        [Required]
        public bool Status { get; set; } = false;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public bool Trending { get; set; } = true;
        [Required]
        public string Tags { get; set; } = string.Empty;
        public DateTime Create_at { get; set; }

        [NotMapped]
        public IFormFile? WallpaperFile { get; set; } 
    }
}
