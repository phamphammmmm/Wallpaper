using System.ComponentModel.DataAnnotations;

namespace Wallpaper.DTO.User
{
    public class UserLogin_DTO : Base_DTO
    {
        public bool KeepLoggedIn { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }   
}
