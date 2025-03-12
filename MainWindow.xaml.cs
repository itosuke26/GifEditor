using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using GifEditor.Services;

namespace GifEditor
{
    public partial class MainWindow : Window
    {
        private string inputGifPath = "";
        private string outputGifPath = "";
        private string inputVideoPath = "";
        private string outputVideoPath = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// GIFファイルを選択（入力パスを設定）
        /// </summary>
        private void OnSelectGifFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "GIFファイル (*.gif)|*.gif|すべてのファイル (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                inputGifPath = openFileDialog.FileName;
                GifInputPath.Text = inputGifPath;

                // 出力ファイルを自動で設定（例: "input.gif" → "input_optimized.gif"）
                string directory = Path.GetDirectoryName(inputGifPath);
                string filenameWithoutExt = Path.GetFileNameWithoutExtension(inputGifPath);
                outputGifPath = Path.Combine(directory, $"{filenameWithoutExt}_optimized.gif");
            }
        }

        /// <summary>
        /// GIFの最適化
        /// </summary>
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
                MessageBox.Show("GIFの最適化が完了しました！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラー: {ex.Message}");
            }
        }

        /// <summary>
        /// GIFの圧縮
        /// </summary>
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

        private void OnCompressGifToTargetSizeClicked(object sender, RoutedEventArgs e)
        {
            // 圧縮処理の実装
            string inputPath = GifInputPath.Text; // 入力ファイルパス
            string outputPath = "compressed_output.gif"; // 出力ファイルパス
            long targetSizeKB = 512; // 目標サイズ (512KB)

            try
            {
                GifService.CompressGifToTargetSize(inputPath, outputPath, targetSizeKB);
                MessageBox.Show("GIFの圧縮が完了しました！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラー: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// GIFの切り抜き
        /// </summary>
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

        /// <summary>
        /// 動画ファイルを選択（入力パスを設定）
        /// </summary>
        private void OnSelectVideoInputFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "動画ファイル (*.mkv;*.mp4)|*.mkv;*.mp4|すべてのファイル (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                inputVideoPath = openFileDialog.FileName;
                VideoInputPath.Text = inputVideoPath;

                // 出力ファイルを自動で設定
                string directory = Path.GetDirectoryName(inputVideoPath);
                string filenameWithoutExt = Path.GetFileNameWithoutExtension(inputVideoPath);

                if (inputVideoPath.EndsWith(".mkv", StringComparison.OrdinalIgnoreCase))
                {
                    outputVideoPath = Path.Combine(directory, $"{filenameWithoutExt}.mp4"); // MKV → MP4
                }
                else if (inputVideoPath.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase))
                {
                    outputVideoPath = Path.Combine(directory, $"{filenameWithoutExt}.gif"); // MP4 → GIF
                }
            }
        }


        /// <summary>
        /// MKV → MP4 変換
        /// </summary>
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

        /// <summary>
        /// MP4 → GIF 変換
        /// </summary>
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
    }
}