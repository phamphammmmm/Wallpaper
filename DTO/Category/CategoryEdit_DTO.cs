using System.ComponentModel.DataAnnotations;

namespace Wallpaper.DTO.Category
{
    public class CategoryEdit_DTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int Sort {  get; set; }
        [Required]
        public DateTime Update_at { get; set; }
        public IFormFile? CategoryFile { get; set; }
    }
}
