using System.ComponentModel.DataAnnotations;

namespace Wallpaper.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int Sort { get; set; }
        public IFormFile? CategoryFile { get; set; }
    }
}
