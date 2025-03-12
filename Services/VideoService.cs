using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace GifEditor.Services
{
    public static class VideoService
    {
        // GIF → MP4 変換
        public static async Task ConvertGifToMp4(string inputPath, string outputPath)
        {
            await FFmpeg.Conversions.New()
                .AddParameter($"-i \"{inputPath}\" -movflags faststart -pix_fmt yuv420p \"{outputPath}\"")
                .Start();
        }

        // MP4 → GIF 変換
        public static async Task ConvertMp4ToGif(string inputPath, string outputPath)
        {
            await FFmpeg.Conversions.New()
                .AddParameter($"-i \"{inputPath}\" -vf \"fps=15,scale=500:-1:flags=lanczos\" \"{outputPath}\"")
                .Start();
        }

        // MKV → MP4 変換
        public static async Task ConvertMkvToMp4(string inputPath, string outputPath)
        {
            await FFmpeg.Conversions.New()
                .AddParameter($"-i \"{inputPath}\" -c:v libx264 -preset slow -crf 23 \"{outputPath}\"")
                .Start();
        }
    }
}
