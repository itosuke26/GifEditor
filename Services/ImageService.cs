using ImageMagick;
using System.IO;

namespace GifEditor.Services
{
    public class ImageService
    {
        // GIF画像のリサイズ
        public static void ResizeGif(string inputPath, string outputPath, uint width, uint height)
        {
            using (var collection = new MagickImageCollection(inputPath))
            {
                foreach (var frame in collection)
                {
                    frame.Resize(width, height); // uint 型に統一
                }
                collection.Write(outputPath);
            }
        }

        // GIF画像の最初のフレームを別の画像形式に変換
        public static void ConvertGifToImage(string inputPath, string outputPath, MagickFormat format)
        {
            using (var collection = new MagickImageCollection(inputPath))
            {
                collection[0].Write(outputPath, format); // 1枚目のフレームを変換
            }
        }
    }
}