using System.IO;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace GifEditor.Services
{
    public static class VideoService
    {
        public static async Task ConvertMp4ToGif(string inputPath, string outputPath)
        {
            await FFmpeg.Conversions.New()
                .AddParameter($"-i \"{inputPath}\" \"{outputPath}\"")
                .Start();
        }

        public static async Task ConvertMkvToMp4(string inputPath, string outputPath)
        {
            await FFmpeg.Conversions.New()
                .AddParameter($"-i \"{inputPath}\" \"{outputPath}\"")
                .Start();
        }

        public static async Task ConvertGifToMp4(string inputPath, string outputPath)
        {
            if (File.Exists(outputPath))
            {
                string directory = Path.GetDirectoryName(outputPath);
                string filenameWithoutExt = Path.GetFileNameWithoutExtension(outputPath);
                string extension = Path.GetExtension(outputPath);
                int count = 1;
                while (File.Exists(Path.Combine(directory, $"{filenameWithoutExt}_{count}{extension}")))
                {
                    count++;
                }
                outputPath = Path.Combine(directory, $"{filenameWithoutExt}_{count}{extension}");
            }

            await FFmpeg.Conversions.New()
                .AddParameter($"-i \"{inputPath}\" \"{outputPath}\"")
                .Start();
        }
    }
}