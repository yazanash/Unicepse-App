using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Uniceps.ViewModels.PlayersViewModels;
using Windows.Devices.Enumeration;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.Capture.Frames;
using Windows.Media.MediaProperties;
using ZXing;
using ZXing.Common;

namespace Uniceps.Views
{
    /// <summary>
    /// Interaction logic for CameraReader.xaml
    /// </summary>
    public sealed record CameraDevice(string Name, string Id);

    public partial class CameraReader : System.Windows.Window
    {
        // Camera
        private MediaCapture? _capture;
        private MediaFrameReader? _frameReader;
        private readonly SemaphoreSlim _lifecycle = new(1, 1); // serializes start/stop
        private bool _closing;

        // Latest frame (BGRA32), shared between the reader thread and the UI thread
        private readonly object _bufLock = new();
        private byte[] _frameBuffer = Array.Empty<byte>();
        private int _frameWidth, _frameHeight;
        private WriteableBitmap? _bitmap;

        // Back-pressure flags (0 = free, 1 = busy)
        private int _processing;
        private int _uiPending;
        private int _decoding;
        private long _lastDecodeTicks;
        private const int DecodeIntervalMs = 300;

        // QR
        private readonly BarcodeReaderGeneric _reader = new()
        {
            AutoRotate = true,
            Options = new DecodingOptions
            {
                TryHarder = true,
                // QR only = faster + fewer false reads. Remove this line if you also scan other barcodes.
                PossibleFormats = new[] { BarcodeFormat.QR_CODE }
            }
        };

        private readonly ReadPlayerQrCodeViewModel? _viewModelBase;
        private readonly bool _stillOpen;

        public CameraReader()
        {
            InitializeComponent();
            Loaded += async (_, _) => await LoadCamerasAsync();
        }

        public CameraReader(bool stillOpen, ReadPlayerQrCodeViewModel viewModelBase) : this()
        {
            _stillOpen = stillOpen;
            _viewModelBase = viewModelBase;
            _viewModelBase.UID = null;
            Topmost = true;
        }

        // ---------- Cameras ----------

        private async Task LoadCamerasAsync()
        {
            try
            {
                var devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

                vidlist.Items.Clear();
                foreach (var d in devices)
                    vidlist.Items.Add(new CameraDevice(d.Name, d.Id));

                if (vidlist.Items.Count == 0)
                {
                    MessageBox.Show("لا توجد كاميرا متصلة");
                    return;
                }

                // Triggers SelectionChanged -> starts the camera (once)
                vidlist.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"تعذر قراءة قائمة الكاميرات: {ex.Message}");
            }
        }

        private async void vidlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (vidlist.SelectedItem is CameraDevice cam)
                await StartCameraAsync(cam.Id);
        }

        private async Task StartCameraAsync(string deviceId)
        {
            await _lifecycle.WaitAsync();
            try
            {
                await StopCoreAsync();
                if (_closing) return;

                var capture = new MediaCapture();
                try
                {
                    await capture.InitializeAsync(new MediaCaptureInitializationSettings
                    {
                        VideoDeviceId = deviceId,
                        StreamingCaptureMode = StreamingCaptureMode.Video,
                        SharingMode = MediaCaptureSharingMode.SharedReadOnly,
                        MemoryPreference = MediaCaptureMemoryPreference.Cpu
                    });

                    var source = capture.FrameSources.Values
                        .FirstOrDefault(s => s.Info.SourceKind == MediaFrameSourceKind.Color)
                        ?? throw new InvalidOperationException("الكاميرا لا تدعم بث الألوان");

                    var reader = await capture.CreateFrameReaderAsync(source, MediaEncodingSubtypes.Bgra8);
                    reader.AcquisitionMode = MediaFrameReaderAcquisitionMode.Realtime;
                    reader.FrameArrived += OnFrameArrived;

                    var status = await reader.StartAsync();
                    if (status != MediaFrameReaderStartStatus.Success)
                    {
                        reader.FrameArrived -= OnFrameArrived;
                        reader.Dispose();
                        throw new InvalidOperationException($"فشل تشغيل الكاميرا ({status})");
                    }

                    _capture = capture;
                    _frameReader = reader;
                }
                catch (UnauthorizedAccessException)
                {
                    capture.Dispose();
                    MessageBox.Show("الوصول للكاميرا مرفوض. فعّل: Settings > Privacy > Camera > Let desktop apps access your camera");
                }
                catch (Exception ex)
                {
                    capture.Dispose();
                    MessageBox.Show($"لا يمكن فتح الكاميرا: {ex.Message}");
                }
            }
            finally
            {
                _lifecycle.Release();
            }
        }

        private async Task StopCameraAsync()
        {
            await _lifecycle.WaitAsync();
            try { await StopCoreAsync(); }
            finally { _lifecycle.Release(); }
        }

        // Call only while holding _lifecycle
        private async Task StopCoreAsync()
        {
            var reader = _frameReader;
            var capture = _capture;
            _frameReader = null;
            _capture = null;

            if (reader != null)
            {
                reader.FrameArrived -= OnFrameArrived;
                try { await reader.StopAsync(); } catch { }
                reader.Dispose();
            }
            capture?.Dispose();
        }

        // ---------- Frames (runs on a background thread) ----------

        private void OnFrameArrived(MediaFrameReader sender, MediaFrameArrivedEventArgs args)
        {
            // Drop the frame if the previous one is still being processed
            if (Interlocked.Exchange(ref _processing, 1) == 1) return;

            try
            {
                if (_closing) return;

                using var frameRef = sender.TryAcquireLatestFrame();
                var src = frameRef?.VideoMediaFrame?.SoftwareBitmap;
                if (src == null) return;

                SoftwareBitmap? converted = null;
                var bmp = src;
                if (src.BitmapPixelFormat != BitmapPixelFormat.Bgra8)
                {
                    converted = SoftwareBitmap.Convert(src, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                    bmp = converted;
                }

                try
                {
                    int w = bmp.PixelWidth, h = bmp.PixelHeight;
                    lock (_bufLock)
                    {
                        int size = w * h * 4;
                        if (_frameBuffer.Length != size) _frameBuffer = new byte[size];
                        bmp.CopyToBuffer(_frameBuffer.AsBuffer());
                        _frameWidth = w;
                        _frameHeight = h;
                    }
                }
                finally
                {
                    converted?.Dispose();
                }

                // Preview: at most one pending UI update at a time
                if (Interlocked.Exchange(ref _uiPending, 1) == 0)
                    Dispatcher.BeginInvoke(RenderFrame);

                // QR decode: throttled, on its own task, never blocks the preview
                long now = Environment.TickCount64;
                if (now - Interlocked.Read(ref _lastDecodeTicks) >= DecodeIntervalMs &&
                    Interlocked.Exchange(ref _decoding, 1) == 0)
                {
                    Interlocked.Exchange(ref _lastDecodeTicks, now);

                    byte[] snapshot;
                    int sw, sh;
                    lock (_bufLock)
                    {
                        snapshot = (byte[])_frameBuffer.Clone();
                        sw = _frameWidth;
                        sh = _frameHeight;
                    }
                    _ = DecodeAsync(snapshot, sw, sh);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"OnFrameArrived: {ex.Message}");
            }
            finally
            {
                Volatile.Write(ref _processing, 0);
            }
        }

        // ---------- UI thread ----------

        private void RenderFrame()
        {
            try
            {
                lock (_bufLock)
                {
                    int w = _frameWidth, h = _frameHeight;
                    if (w == 0 || h == 0) return;

                    if (_bitmap == null || _bitmap.PixelWidth != w || _bitmap.PixelHeight != h)
                    {
                        _bitmap = new WriteableBitmap(w, h, 96, 96, PixelFormats.Bgra32, null);
                        camera_img.Source = _bitmap;
                    }
                    _bitmap.WritePixels(new Int32Rect(0, 0, w, h), _frameBuffer, w * 4, 0);
                }
            }
            finally
            {
                Volatile.Write(ref _uiPending, 0);
            }
        }

        private async Task DecodeAsync(byte[] data, int width, int height)
        {
            try
            {
                string? text = await Task.Run(() =>
                {
                    try { return _reader.Decode(data, width, height, RGBLuminanceSource.BitmapFormat.BGRA32)?.Text; }
                    catch { return null; }
                });

                if (string.IsNullOrEmpty(text) || _closing) return;
                await Dispatcher.InvokeAsync(() => OnQrDetected(text));
            }
            finally
            {
                Volatile.Write(ref _decoding, 0);
            }
        }

        private void OnQrDetected(string text)
        {
            if (txt_toview.Text == text) return; // same code still in front of the camera

            txt_toview.Text = text;
            PlaySoundFromResources();

            if (_viewModelBase != null)
                _viewModelBase.OnCatchChanged();
            else if (!_stillOpen)
                Close();
        }

        // ---------- Misc ----------

        public void PlaySoundFromResources()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                Stream? soundStream = assembly.GetManifestResourceStream("Eplus.Resources.sounds.beep.wav");
                if (soundStream != null)
                    new SoundPlayer(soundStream).Play();
            }
            catch { }
        }

        // Kept only because the XAML may still reference them
        private void Button_Click(object sender, RoutedEventArgs e) { }
        private void Button_Click_1(object sender, RoutedEventArgs e) { }

        protected override void OnClosed(EventArgs e)
        {
            _closing = true;
            _ = StopCameraAsync();
            base.OnClosed(e);
        }
    }
}
