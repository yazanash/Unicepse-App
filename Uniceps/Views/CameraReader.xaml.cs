using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Uniceps.ViewModels;
using Uniceps.ViewModels.PlayersViewModels;
//using AForge.Video;
//using AForge.Video.DirectShow;
using ZXing;
using ZXing.Aztec.Internal;
using ZXing.Common;
using ZXing.Windows.Compatibility;

namespace Uniceps.Views
{
    /// <summary>
    /// Interaction logic for CameraReader.xaml
    /// </summary>
    class Camera : ViewModelBase
    {
        public int Index { get; set; }
        public Camera(string name, int index)
        {
            Name = name;
            Index = index;
        }
        public string Name { get; set; }
    }
    public partial class CameraReader : System.Windows.Window
    {
        private VideoCapture? _capture;
        private DispatcherTimer? _timer;
        private BarcodeReader? _reader;
        private CancellationTokenSource? _cts;
        private DateTime _lastQrCheck = DateTime.MinValue;

        ReadPlayerQrCodeViewModel? _viewModelBase;
        bool _stillOpen= false;
        public CameraReader()
        {
            InitializeComponent();
            LoadCameras();
            vidlist.SelectedIndex = 0;
        }
        public CameraReader(bool stillOpen, ReadPlayerQrCodeViewModel viewModelBase)
        {
            InitializeComponent();
            _stillOpen=stillOpen;
            _viewModelBase = viewModelBase;
            _viewModelBase.UID = null;
            LoadCameras();
            vidlist.SelectedIndex = 0;
            this.Topmost = true;
        }
        private void LoadCameras()
        {
            vidlist.Items.Clear();
            for (int i = 0; i < 5; i++) // حاول نعرض أول 5 كاميرات
            {
                using var tempCapture = new VideoCapture(i);
                if (tempCapture.IsOpened())
                {
                    vidlist.Items.Add(new Camera($"Camera {i}", i));
                    tempCapture.Release();
                }
            }
            if (vidlist.Items.Count > 0)
                vidlist.SelectedIndex = 0;
        }
        private void StartCamera(int index)
        {
            StopCamera();

            _capture = new VideoCapture(index);
            if (!_capture.IsOpened())
            {
                MessageBox.Show("لا يمكن فتح الكاميرا");
                return;
            }

            _reader = new BarcodeReader
            {
                AutoRotate = true,
                Options = new DecodingOptions { TryHarder = true }
            };

            _cts = new CancellationTokenSource();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(30)
            };
            _timer.Tick += async (s, e) => await CaptureFrame(s, e, _cts.Token);
            _timer.Start();
        }

        private async Task CaptureFrame(object? sender, EventArgs e, CancellationToken token)
        {
            if (token.IsCancellationRequested) return;
            if (_capture == null || !_capture.IsOpened()) return;

            try
            {
                using var frame = new Mat();
                _capture.Read(frame);

                if (frame.Empty() || frame.Width == 0 || frame.Height == 0) return;

                using var bmp = BitmapConverter.ToBitmap(frame);

                // تحديث الصورة على UI
                camera_img.Source = ConvertToBitmapSource(bmp);

                // Throttle: قراءة QR كل 500ms فقط
                if ((DateTime.Now - _lastQrCheck).TotalMilliseconds < 500)
                    return;

                _lastQrCheck = DateTime.Now;

                string? qrText = await Task.Run(() =>
                {
                    if (bmp == null || bmp.Width == 0 || bmp.Height == 0)
                        return null;

                    try
                    {
                        var result = _reader?.Decode(bmp);
                        return result?.Text;
                    }
                    catch
                    {
                        return null;
                    }
                }, token);

                if (!string.IsNullOrEmpty(qrText)&& txt_toview.Text != qrText)
                {
                    txt_toview.Text = qrText;
                    PlaySoundFromResources();

                    if (_viewModelBase != null)
                        _viewModelBase.OnCatchChanged();
                    else if (!_stillOpen)
                        CloseWindow();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CaptureFrame: {ex.Message}");
            }
        }
        private void StopCamera()
        {
            try
            {
                _cts?.Cancel();
                _timer?.Stop();
                _capture?.Release();
                _capture?.Dispose();
            }
            catch { }
        }
        private void CloseWindow()
        {
            StopCamera();
            this.Close();
        }

        public void PlaySoundFromResources()
        {
            // Get the current assembly
            //var assembly = Assembly.GetExecutingAssembly();
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                // Get the stream for the embedded resource
                Stream? soundStream = assembly.GetManifestResourceStream("Uniceps.Resources.sounds.beep.wav");

                if (soundStream != null)
                {
                    // Create a SoundPlayer instance
                    SoundPlayer player = new SoundPlayer(soundStream);

                    // Play the sound
                    player.Play();
                }
                else
                {
                    MessageBox.Show("Sound resource not found.");
                }
            }
            catch { }
        }
        private BitmapSource ConvertToBitmapSource(Bitmap bitmap)
        {
            using var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Bmp);
            memoryStream.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            StopCamera();
            base.OnClosing(e);
        }
       

        private void vidlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (vidlist.SelectedItem is Camera cam)
            {
                StartCamera(cam.Index);
            }
        }
    }
}
