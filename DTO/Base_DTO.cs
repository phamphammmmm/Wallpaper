using System.ComponentModel.DataAnnotations;

namespace Wallpaper.DTO
{
    public class Base_DTO
    {
        [Required]
        public string Name { get; set; }= string.Empty;
    }
}
