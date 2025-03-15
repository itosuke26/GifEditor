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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnSelectGifFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "GIFファイル (*.gif)|*.gif|すべてのファイル (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                inputGifPath = openFileDialog.FileName;
                GifInputPath.Text = inputGifPath; // 選択したGIFファイルのパスをテキストボックスに表示

                string directory = Path.GetDirectoryName(inputGifPath);
                string filenameWithoutExt = Path.GetFileNameWithoutExtension(inputGifPath);
                outputGifPath = Path.Combine(directory, $"{filenameWithoutExt}_compressed.gif");
            }
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

        private void OnCompressGifClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(inputGifPath) || string.IsNullOrEmpty(outputGifPath))
                {
                    MessageBox.Show("入力または出力のパスが設定されていません。");
                    return;
                }

                GifService.CompressGif(inputGifPath, outputGifPath);
                MessageBox.Show("GIFの圧縮が完了しました！");
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

                if (!int.TryParse(ResizeWidthInput.Text, out int width) || !int.TryParse(ResizeHeightInput.Text, out int height))
                {
                    MessageBox.Show("無効なリサイズサイズです。正しい数値を入力してください。");
                    return;
                }

                GifService.ResizeGif(inputGifPath, outputGifPath, width, height);
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
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "動画ファイル (*.mkv;*.mp4;*.gif)|*.mkv;*.mp4;*.gif|すべてのファイル (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                inputVideoPath = openFileDialog.FileName;
                VideoInputPath.Text = inputVideoPath; // 選択した動画ファイルのパスをテキストボックスに表示

                string directory = Path.GetDirectoryName(inputVideoPath);
                string filenameWithoutExt = Path.GetFileNameWithoutExtension(inputVideoPath);

                if (inputVideoPath.EndsWith(".mkv", StringComparison.OrdinalIgnoreCase))
                {
                    outputVideoPath = Path.Combine(directory, $"{filenameWithoutExt}.mp4");
                }
                else if (inputVideoPath.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase))
                {
                    outputVideoPath = Path.Combine(directory, $"{filenameWithoutExt}.gif");
                }
                else if (inputVideoPath.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    outputVideoPath = Path.Combine(directory, $"{filenameWithoutExt}.mp4");
                }
            }
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

        private async void OnConvertGifToMp4Clicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(inputGifPath))
            {
                MessageBox.Show("入力GIFファイルが選択されていません。");
                return;
            }

            string directory = Path.GetDirectoryName(inputGifPath);
            string filenameWithoutExt = Path.GetFileNameWithoutExtension(inputGifPath);
            string newOutputVideoPath = Path.Combine(directory, $"{filenameWithoutExt}.mp4");

            try
            {
                await VideoService.ConvertGifToMp4(inputGifPath, newOutputVideoPath);
                MessageBox.Show("GIF → MP4 変換が完了しました！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラー: {ex.Message}");
            }
        }
        private void OnSelectImageServiceGifFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "GIFファイル (*.gif)|*.gif|すべてのファイル (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                imageServiceInputGifPath = openFileDialog.FileName;
                ImageServiceGifInputPath.Text = imageServiceInputGifPath;
                string directory = Path.GetDirectoryName(imageServiceInputGifPath);
                string filenameWithoutExt = Path.GetFileNameWithoutExtension(imageServiceInputGifPath);
                imageServiceOutputImagePath = Path.Combine(directory, $"{filenameWithoutExt}.png"); // デフォルトの出力形式
            }
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