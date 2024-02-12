using System.ComponentModel.DataAnnotations;

namespace Wallpaper.DTO.User
{
    public class UserCreate_DTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public DateTime Create_at { get; set; }
    }
}
