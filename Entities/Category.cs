using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallpaper.Entities
{
    public class Category : Activity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int Sort { get; set; }
        [Required]
        public string Preview { get; set; } = string.Empty;
        public ICollection<Wallpaper> Wallpapers { get; set; } = new List<Wallpaper>();

    }
}
