namespace Wallpaper.Service
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile file, string folderName);
        string GetFileName(IFormFile file);
        long GetFileSize(IFormFile file);
        string GetFileExtension(IFormFile file);
        string GetRatioOfFile(IFormFile file);
        int GCD(int a, int b);
    }
}
