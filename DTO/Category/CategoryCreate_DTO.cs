using System.ComponentModel.DataAnnotations;

namespace Wallpaper.DTO.Category
{
    public class CategoryCreate_DTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int Sort { get; set; } 
        [Required]
        public DateTime Create_at { get; set; }
        public IFormFile? CategoryFile { get; set; }
    }
}
