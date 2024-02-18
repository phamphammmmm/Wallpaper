using System.ComponentModel.DataAnnotations;

namespace Wallpaper.Entities
{
    public class Base
    {
        [Key]
        public int Id { get; set; }
        public DateTime Create_at { get; set; }
        public DateTime Update_at { get; set; }
    }
}
