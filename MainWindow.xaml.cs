using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using GifEditor.Services;
using ImageMagick;
using System.Windows.Controls;

namespace GifEditor
{
    public partial class MainWindow : Window
    {
        private string inputGifPath = "";
        private string outputGifPath = "";
        private string inputVideoPath = "";
        private string outputVideoPath = "";
        private string imageServiceInputGifPath = "";
        private string imageServiceOutputImagePath = "";
        private string gifInputPathForVideo = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnOptimizeGifClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(inputGifPath) || string.IsNullOrEmpty(outputGifPath))
                {
                    MessageBox.Show("入力または出力のパスが設定されていません。");
                    return;
                }

                if (!int.TryParse(FrameRateInput.Text, out int frameRate) || frameRate <= 0)
                {
                    MessageBox.Show("無効なフレームレートです。正しい数値を入力してください。");
                    return;
                }

                bool optimizePalette = OptimizePaletteCheckBox.IsChecked ?? false;
                GifService.OptimizeGif(inputGifPath, outputGifPath, frameRate, optimizePalette);
                MessageBox.Show("FPS,カラーパレット最適化が完了しました！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラー: {ex.Message}");
            }
        }

        private void OnResizeGifClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(inputGifPath) || string.IsNullOrEmpty(outputGifPath))
                {
                    MessageBox.Show("入力または出力のパスが設定されていません。");
                    return;
                }

                if (!uint.TryParse(ResizeWidthInput.Text, out uint width) || !uint.TryParse(ResizeHeightInput.Text, out uint height))
                {
                    MessageBox.Show("無効なリサイズサイズです。正しい数値を入力してください。");
                    return;
                }
                GifService.ResizeGif(inputGifPath, outputGifPath, (int)width, (int)height);
                MessageBox.Show("GIFのリサイズが完了しました！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラー: {ex.Message}");
            }
        }

        private void OnCompressGifToTargetSizeClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(inputGifPath) || string.IsNullOrEmpty(outputGifPath))
                {
                    MessageBox.Show("入力または出力のパスが設定されていません。");
                    return;
                }

                if (!long.TryParse(TargetSizeInput.Text, out long targetSizeKB))
                {
                    MessageBox.Show("無効な目標サイズです。正しい数値を入力してください。");
                    return;
                }

                GifService.CompressGifToTargetSize(inputGifPath, outputGifPath, targetSizeKB);
                MessageBox.Show("GIFのサイズ圧縮が完了しました！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラー: {ex.Message}");
            }
        }

        private void OnCropGifClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(inputGifPath) || string.IsNullOrEmpty(outputGifPath))
                {
                    MessageBox.Show("入力または出力のパスが設定されていません。");
                    return;
                }

                if (!int.TryParse(XInput.Text, out int x) ||
                    !int.TryParse(YInput.Text, out int y) ||
                    !int.TryParse(WidthInput.Text, out int width) ||
                    !int.TryParse(HeightInput.Text, out int height) ||
                    width <= 0 || height <= 0)
                {
                    MessageBox.Show("無効な切り抜きサイズです。正しい数値を入力してください。");
                    return;
                }

                GifService.CropGif(inputGifPath, outputGifPath, x, y, width, height);
                MessageBox.Show("GIFの切り抜きが完了しました！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラー: {ex.Message}");
            }
        }

        private void OnSelectVideoInputFile(object sender, RoutedEventArgs e)
        {
            SelectFile(VideoInputPath, "動画ファイル (*.mkv;*.mp4;*.gif)|*.mkv;*.mp4;*.gif|すべてのファイル (*.*)|*.*", filePath =>
            {
                inputVideoPath = filePath;
                if (filePath.EndsWith(".mkv", StringComparison.OrdinalIgnoreCase))
                {
                    outputVideoPath = GenerateOutputFilePath(filePath, ".mp4");
                }
                else if (filePath.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase))
                {
                    outputVideoPath = GenerateOutputFilePath(filePath, ".gif");
                }
                else if (filePath.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    outputVideoPath = GenerateOutputFilePath(filePath, ".mp4");
                }
            });
        }

        private async void OnConvertMkvToMp4Clicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(inputVideoPath) || string.IsNullOrEmpty(outputVideoPath))
            {
                MessageBox.Show("入力または出力のパスが設定されていません。");
                return;
            }

            await VideoService.ConvertMkvToMp4(inputVideoPath, outputVideoPath);
            MessageBox.Show("MKV → MP4 変換が完了しました！");
        }

        private async void OnConvertMp4ToGifClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(inputVideoPath) || string.IsNullOrEmpty(outputVideoPath))
            {
                MessageBox.Show("入力または出力のパスが設定されていません。");
                return;
            }

            await VideoService.ConvertMp4ToGif(inputVideoPath, outputVideoPath);
            MessageBox.Show("MP4 → GIF 変換が完了しました！");
        }

        private void OnSelectGifForVideo(object sender, RoutedEventArgs e)
        {
            SelectFile(GifInputPathForVideo, "GIFファイル (*.gif)|*.gif|すべてのファイル (*.*)|*.*", filePath =>
            {
                gifInputPathForVideo = filePath;
            });
        }
        private void OnSelectImageServiceGifFile(object sender, RoutedEventArgs e)
        {
            SelectFile(ImageServiceGifInputPath, "GIFファイル (*.gif)|*.gif|すべてのファイル (*.*)|*.*", filePath =>
            {
                imageServiceInputGifPath = filePath;
                string directory = Path.GetDirectoryName(filePath);
                string filenameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
                imageServiceOutputImagePath = Path.Combine(directory, $"{filenameWithoutExt}_resized.gif");
            });
        }
        private async void OnConvertGifToMp4Clicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(gifInputPathForVideo))
            {
                MessageBox.Show("入力GIFファイルが選択されていません。");
                return;
            }

            string directory = Path.GetDirectoryName(gifInputPathForVideo);
            string filenameWithoutExt = Path.GetFileNameWithoutExtension(gifInputPathForVideo);
            string newOutputVideoPath = Path.Combine(directory, $"{filenameWithoutExt}.mp4");

            try
            {
                await VideoService.ConvertGifToMp4(gifInputPathForVideo, newOutputVideoPath);
                MessageBox.Show("GIF → MP4 変換が完了しました！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラー: {ex.Message}");
            }
        }
        private void SelectFile(TextBox textBox, string filter, Action<string> filePathCallback)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = filter };
            if (openFileDialog.ShowDialog() == true)
            {
                textBox.Text = openFileDialog.FileName;
                filePathCallback(openFileDialog.FileName);
            }
        }

        private string GenerateOutputFilePath(string inputFilePath, string extension)
        {
            string directory = Path.GetDirectoryName(inputFilePath);
            string filenameWithoutExt = Path.GetFileNameWithoutExtension(inputFilePath);
            return Path.Combine(directory, $"{filenameWithoutExt}{extension}");
        }

        private void OnSelectGifFile(object sender, RoutedEventArgs e)
        {
            SelectFile(GifInputPath, "GIFファイル (*.gif)|*.gif|すべてのファイル (*.*)|*.*", filePath =>
            {
                inputGifPath = filePath;
                outputGifPath = GenerateOutputFilePath(filePath, "_compressed.gif");
            });
        }

        private void OnImageServiceResizeGifClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(imageServiceInputGifPath))
                {
                    MessageBox.Show("入力GIFファイルを選択してください。");
                    return;
                }

                if (!uint.TryParse(ImageServiceResizeWidthInput.Text, out uint width) ||
                    !uint.TryParse(ImageServiceResizeHeightInput.Text, out uint height))
                {
                    MessageBox.Show("無効なリサイズサイズです。正しい数値を入力してください。");
                    return;
                }

                ImageService.ResizeGif(imageServiceInputGifPath, imageServiceOutputImagePath, width, height);
                MessageBox.Show("GIFのリサイズが完了しました！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラー: {ex.Message}");
            }
        }

        private void OnImageServiceConvertGifToImageClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(imageServiceInputGifPath))
                {
                    MessageBox.Show("入力GIFファイルを選択してください。");
                    return;
                }

                MagickFormat format = MagickFormat.Png; // デフォルト
                if (ImageServiceOutputFormatComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    if (selectedItem.Content.ToString() == "JPEG")
                    {
                        format = MagickFormat.Jpeg;
                    }
                    else if (selectedItem.Content.ToString() == "BMP")
                    {
                        format = MagickFormat.Bmp;
                    }
                }

                string directory = Path.GetDirectoryName(imageServiceInputGifPath);
                string filenameWithoutExt = Path.GetFileNameWithoutExtension(imageServiceInputGifPath);
                string outputImagePath = Path.Combine(directory, $"{filenameWithoutExt}.{format.ToString().ToLower()}");

                ImageService.ConvertGifToImage(imageServiceInputGifPath, outputImagePath, format);
                MessageBox.Show("最初のフレームの変換が完了しました！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラー: {ex.Message}");
            }
        }
    }
}