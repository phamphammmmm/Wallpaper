namespace Wallpaper.DTO.Category
{
    public class CategoryList_DTO
    {
        public List<Entities.Category>? Categories { get; set; }
        public string Keyword { get; set; } = string.Empty;
        public int? Sort {  get; set; }
        public string ColName { get; set; } = string.Empty;
        public bool IsAsc { get; set; }
        public int Index { get; set; }
        public int Size { get; set; }
        public int TotalPages { get; set; }
    }
}
