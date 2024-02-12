using System.ComponentModel.DataAnnotations;

namespace Wallpaper.DTO.Tag
{
    public class TagEdit_DTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public DateTime Update_at { get; set; }
    }
}
