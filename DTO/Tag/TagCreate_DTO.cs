using System.ComponentModel.DataAnnotations;

namespace Wallpaper.DTO.Tag
{
    public class TagCreate_DTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateTime Create_at { get; set; }
    }
}
