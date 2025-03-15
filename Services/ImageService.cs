using ImageMagick;
using System;
using System.IO;
using System.Windows; // MessageBox を使用する場合

namespace GifEditor.Services
{
    public class ImageService
    {
        // GIF画像のリサイズ
        public static void ResizeGif(string inputPath, string outputPath, uint width, uint height)
        {
            try
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
            catch (MagickException ex)
            {
                // ImageMagick 固有の例外をキャッチ
                MessageBox.Show($"ImageMagick エラー: {ex.Message}");
            }
            catch (Exception ex)
            {
                // その他の例外をキャッチ
                MessageBox.Show($"エラー: {ex.Message}");
            }
        }

        // GIF画像の最初のフレームを別の画像形式に変換
        public static void ConvertGifToImage(string inputPath, string outputPath, MagickFormat format)
        {
            try
            {
                using (var collection = new MagickImageCollection(inputPath))
                {
                    collection[0].Write(outputPath, format); // 1枚目のフレームを変換
                }
            }
            catch (MagickException ex)
            {
                MessageBox.Show($"ImageMagick エラー: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラー: {ex.Message}");
            }
        }
    }
}