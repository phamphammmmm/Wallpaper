using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Wallpaper.Service
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveImageAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Invalid file");
            }

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            var filePath = Path.Combine(folderPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/{folderName}/{uniqueFileName}";
        }

        public long GetFileSize(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                long length = stream.Length;
                return length;
            }
        }

        public string GetFileExtension(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), "File is null");
            }

            using (var stream = file.OpenReadStream())
            {
                if (stream.Length < 1)
                {
                    throw new ArgumentException("File stream is empty", nameof(file));
                }

                // Get the file extension using the content type
                string ext = Path.GetExtension(file.FileName);
                return ext;
            }
        }

        public string GetRatioOfFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Invalid file");
            }

            using (var image = Image.Load<Rgba32>(file.OpenReadStream()))
            {
                int width = image.Width;
                int height = image.Height;

                // Tính tỉ lệ
                int gcd = GCD(height, width);
                int normalizedHeight = height / gcd;
                int normalizedWidth = width / gcd;

                return $"{normalizedHeight}:{normalizedWidth}";
            }
        }

        public int GCD(int a, int b)
        {
            return b == 0 ? Math.Abs(a) : GCD(b, a % b);
        }

        public string GetFileName(IFormFile file)
        {
            var FileName = Path.GetFileName(file.FileName);
            return FileName;
        }
    }
}
