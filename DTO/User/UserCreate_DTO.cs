using System.ComponentModel.DataAnnotations;

namespace Wallpaper.DTO.User
{
    public class UserCreate_DTO : Base_DTO
    {
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime Create_at { get ; set ; }
        public DateTime Update_at { get ; set ; }
    }
}
