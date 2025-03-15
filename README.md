# GIF編集ツール

GIF編集ツールは、動画ファイル（MKV, MP4）からGIFアニメーションを作成・編集するためのツールです。

## 機能

* 動画ファイル（MKV, MP4）からGIFアニメーションへの変換
* GIFアニメーションのフレームレート調整
* GIFアニメーションのカラーパレット最適化
* GIFアニメーションの切り抜き
* GIFアニメーションのリサイズ
* GIFアニメーションのファイルサイズ削減
* GIFアニメーションの画像ファイルへの出力

## 必要な環境

* [.NET Desktop Runtime 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
* [FFmpeg](https://ffmpeg.org/download.html)（インストールパスを環境変数`PATH`に追加してください）

## 使用方法

1.  アプリケーションを起動します。
2.  「GIF編集」タブで、GIFアニメーションファイルを読み込み、必要な編集操作を行います。
3.  「動画変換」タブで、動画ファイル（MKV, MP4）を読み込み、GIFアニメーションに変換します。
4.  「画像処理」タブで、GIFアニメーションを読み込み、画像ファイルへと出力します。

## 開発環境

* [Visual Studio 2022 Community](https://visualstudio.microsoft.com/ja/vs/community/)
* [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

## 依存ライブラリ

* [Magick.NET-Q16-AnyCPU](https://www.nuget.org/packages/Magick.NET-Q16-AnyCPU/) (ライセンス: [Magick.NETライセンス](https://magick.codeplex.com/license))
* [Magick.NET.Core](https://www.nuget.org/packages/Magick.NET.Core/) (ライセンス: [Magick.NETライセンス](https://magick.codeplex.com/license))
* [Xabe.FFmpeg](https://www.nuget.org/packages/Xabe.FFmpeg/) (ライセンス: [MIT License](https://github.com/tomaszzmuda/Xabe.FFmpeg/blob/master/LICENSE))

## 生成AIによるコード生成

本プロジェクトの一部コードは、以下の生成AIによって生成されています。

* ChatGPT
* Gemini
* Copilot

生成されたコードは、必要に応じて修正および調整されています。

## OBS Studio連携

OBS Studioで録画したファイルをGIFアニメーションに変換し、UnityRoomにアップロードすることで、ゲームプレイ動画などを簡単に共有できます。

* [OBS Studio](https://obsproject.com/ja) (ライセンス: [GPL-2.0-or-later](https://github.com/obsproject/obs-studio/blob/master/COPYING))

## ライセンス

このソフトウェアは、MITライセンスの下で公開されています。

## 開発者

itosuke26