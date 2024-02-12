using System.ComponentModel.DataAnnotations;

namespace Wallpaper.DTO.User
{
    public class UserLogin_DTO
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public bool KeepLoggedIn { get; set; } 
    }
}
