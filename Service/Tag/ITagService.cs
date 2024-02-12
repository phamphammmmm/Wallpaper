namespace Wallpaper.Service.Tag
{
    public interface ITagService
    {
        Task<int[]> GetOrCreateTagsAsync(string[] tags);
    }
}
