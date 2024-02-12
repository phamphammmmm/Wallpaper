using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;
using Wallpaper.DTO.Wallpepar;

namespace Wallpaper.Service.Thumbnail
{
    public class ThumbnailService : IThumbnailService 
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        //private readonly IThumbnailSize _thumbnailSize;
        private readonly IImageService _imageService;

        public ThumbnailService(IWebHostEnvironment webHostEnvironment,
                                IImageService imageService)
        {
            _webHostEnvironment = webHostEnvironment;
            //_thumbnailSize = thumbnailSize;
            _imageService = imageService;
        }

        public string CreateThumbnailFromImage(IFormFile file, string folderName)
        {
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Tạo unique file name bằng cách sử dụng Guid
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            var thumbnailPath = $"{folderName}/{uniqueFileName}";

            var filePath = Path.Combine(folderPath, uniqueFileName);

            using (var image = Image.Load<Rgba32>(file.OpenReadStream()))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(100, 100),
                    Mode = ResizeMode.Max
                }));

                image.Save(filePath);
            }

            return thumbnailPath;
        }

        public async Task<string> CreateThumbnailFromVideo(IFormFile file, string folderName)
        {
            string ffmpegPath = Constants.FfmpegExeFilePath;

            if (!File.Exists(ffmpegPath))
            {
                Console.WriteLine("Please make sure that the \"FfmpegExeFilePath\" constant contains the location of \"ffmpeg.exe\" on your system!");
                Console.WriteLine($"Current value is \"{ffmpegPath}\"");
                return null;
            }

            // Save the uploaded video file to a temporary location
            var tempFilePath = Path.GetTempFileName();
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Generate unique file name for the thumbnail
            var uniqueFileName = Guid.NewGuid().ToString() + ".jpg"; // Output thumbnail as JPEG image
            var thumbnailPath = $"/{folderName}/{uniqueFileName}";
            var outputFilePath = Path.Combine(_webHostEnvironment.WebRootPath, folderName, uniqueFileName);

            // Prepare FFmpeg command to extract a frame from the video as a thumbnail
            string arguments = $"-i \"{tempFilePath}\" -vframes 1 -vf scale={Constants.DEFAULT_WIDTH_VIDEO}:{Constants.DEFAULT_HEIGHT_VIDEO} \"{outputFilePath}\"";

            // Run FFmpeg command
            Process process = new Process
            {
                StartInfo =
                {
                    FileName = ffmpegPath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            process.WaitForExit(); // Wait for the process to finish

            // Cleanup: delete the temporary file
            File.Delete(tempFilePath);

            return thumbnailPath;
        }


        public static class Constants
        {
            public const string FfmpegExeFilePath = @"C:\FFmpeg\bin\ffmpeg.exe";
            public const int SizeDivider = 3;
            public const int DEFAULT_HEIGHT_VIDEO = 100;
            public const int DEFAULT_WIDTH_VIDEO = 100;
        }
    }
}
