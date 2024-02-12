//// ThumbnailSize.cs
//using System;
//using System.Diagnostics;
//using System.IO;
//using Microsoft.AspNetCore.Http;

//namespace Wallpaper.Service.Thumbnail
//{
//    public class ThumbnailSize : IThumbnailSize
//    {
//        private readonly int _originalWidth;
//        private readonly int _originalHeight;
//        private readonly int _sizeDivider;

//        public ThumbnailSize(int originalWidth, int originalHeight, int sizeDivider)
//        {
//            _originalWidth = originalWidth;
//            _originalHeight = originalHeight;
//            _sizeDivider = sizeDivider;
//        }

//        public int GCD(int a, int b)
//        {
//            return b == 0 ? Math.Abs(a) : GCD(b, a % b);
//        }

//        public string GetRatioOfVideo(string filePath)
//        {
//            (int width, int height) = GetVideoDimensions(filePath);

//            int gcd = GCD(height, width);
//            int normalizedHeight = height / gcd;
//            int normalizedWidth = width / gcd;

//            return $"{normalizedHeight}:{normalizedWidth}";
//        }

//        public static (int width, int height) GetVideoDimensions(string filePath)
//        {
//            string ffmpegPath = @"C:\FFmpeg\bin\ffmpeg.exe";
//            string tempFilePath = Path.GetTempFileName();

//            File.Copy(filePath, tempFilePath, true);

//            string arguments = $"-i \"{tempFilePath}\" -v error -select_streams v:0 -show_entries stream=width,height -of csv=s=x:p=0";

//            Process process = new Process();
//            process.StartInfo.FileName = ffmpegPath;
//            process.StartInfo.Arguments = arguments;
//            process.StartInfo.RedirectStandardOutput = true;
//            process.StartInfo.UseShellExecute = false;
//            process.StartInfo.CreateNoWindow = true;

//            process.Start();
//            string output = process.StandardOutput.ReadToEnd();
//            process.WaitForExit();

//            string[] dimensions = output.Split('x');
//            int width = int.Parse(dimensions[0]);
//            int height = int.Parse(dimensions[1]);

//            File.Delete(tempFilePath);

//            return (width, height);
//        }
//    }
//}
