using System;
using System.IO;
using ImageMagick;

namespace GifEditor.Services
{
    public static class GifService
    {
        public static void OptimizeGif(string inputPath, string outputPath, int frameRate, bool optimizePalette)
        {
            try
            {
                using (var collection = new MagickImageCollection(inputPath))
                {
                    int delay = Math.Max(1, 100 / Math.Max(1, frameRate));
                    foreach (var image in collection)
                    {
                        image.AnimationDelay = (uint)delay;
                    }

                    if (optimizePalette)
                    {
                        var settings = new QuantizeSettings
                        {
                            Colors = 256,
                            DitherMethod = DitherMethod.FloydSteinberg
                        };
                        collection.Quantize(settings);
                    }
                    collection.Write(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GIFの最適化に失敗しました: {ex.Message}");
                throw new Exception($"GIFの最適化に失敗しました: {ex.Message}");
            }
        }

        public static void CompressGif(string inputPath, string outputPath)
        {
            try
            {
                using (var collection = new MagickImageCollection(inputPath))
                {
                    collection.Coalesce();
                    collection.Optimize();
                    collection.Write(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GIFの圧縮に失敗しました: {ex.Message}");
                throw new Exception($"GIFの圧縮に失敗しました: {ex.Message}");
            }
        }

        public static void CropGif(string inputPath, string outputPath, int x, int y, int width, int height)
        {
            try
            {
                using (var collection = new MagickImageCollection(inputPath))
                {
                    collection.Coalesce();
                    foreach (var image in collection)
                    {
                        var cropGeometry = new MagickGeometry(x, y, (uint)width, (uint)height)
                        {
                            IgnoreAspectRatio = true
                        };
                        ((MagickImage)image).Crop(cropGeometry);
                        ((MagickImage)image).ResetPage();
                    }
                    collection.Write(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GIFの切り抜きに失敗しました: {ex.Message}");
                throw new Exception($"GIFの切り抜きに失敗しました: {ex.Message}");
            }
        }

        public static void ResizeGif(string inputPath, string outputPath, int width, int height)
        {
            try
            {
                using (var collection = new MagickImageCollection(inputPath))
                {
                    collection.Coalesce();
                    foreach (var frame in collection)
                    {
                        frame.Resize((uint)Math.Max(1, width), (uint)Math.Max(1, height));
                    }
                    collection.Write(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GIFのリサイズに失敗しました: {ex.Message}");
                throw new Exception($"GIFのリサイズに失敗しました: {ex.Message}");
            }
        }

        public static void CompressGifToTargetSize(string inputPath, string outputPath, long targetSizeKB = 512)
        {
            try
            {
                using (var collection = new MagickImageCollection(inputPath))
                {
                    collection.Coalesce();
                    collection.Optimize();

                    int colorCount = 256;
                    while (colorCount > 16)
                    {
                        var settings = new QuantizeSettings
                        {
                            Colors = (uint)colorCount,
                            DitherMethod = DitherMethod.FloydSteinberg
                        };
                        collection.Quantize(settings);

                        using (var memoryStream = new MemoryStream())
                        {
                            collection.Write(memoryStream, MagickFormat.Gif);
                            if (memoryStream.Length / 1024 <= targetSizeKB)
                            {
                                break;
                            }
                        }
                        colorCount /= 2;
                    }

                    for (int i = collection.Count - 1; i > 0; i -= 2)
                    {
                        collection.RemoveAt(i);
                    }

                    foreach (var frame in collection)
                    {
                        frame.Resize(new MagickGeometry("75%"));
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        collection.Write(memoryStream, MagickFormat.Gif);
                        while (memoryStream.Length / 1024 > targetSizeKB)
                        {
                            foreach (var frame in collection)
                            {
                                frame.Resize(new MagickGeometry("90%"));
                            }
                            memoryStream.SetLength(0);
                            collection.Write(memoryStream, MagickFormat.Gif);
                        }
                    }
                    collection.Write(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GIFのサイズ圧縮に失敗しました: {ex.Message}");
                throw new Exception($"GIFのサイズ圧縮に失敗しました: {ex.Message}");
            }
        }
    }
}