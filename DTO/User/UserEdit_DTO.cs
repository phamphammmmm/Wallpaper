using System.ComponentModel.DataAnnotations;

namespace Wallpaper.DTO.User
{
    public class UserEdit_DTO : Base_DTO
    { 
        [Required]
        public int Id { get; set; }
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required] 
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
        public DateTime Update_at { get; set; } = DateTime.UtcNow;
    }
}
