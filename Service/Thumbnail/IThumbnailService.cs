namespace Wallpaper.Service.Thumbnail
{
    public interface IThumbnailService
    {
        string CreateThumbnailFromImage(IFormFile file, string folderName);
        Task<string> CreateThumbnailFromVideo(IFormFile file, string folderName);
    }
}
