using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using System.Media;
using System.Drawing;
using ZXing.Windows.Compatibility;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;

namespace Unicepse.WPF
{
    /// <summary>
    /// Interaction logic for CameraReader.xaml
    /// </summary>
    public partial class CameraReader : Window
    {
        FilterInfoCollection videoDevices;
        VideoCaptureDevice videoSource;
        public CameraReader()
        {
            InitializeComponent();
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);

        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            // Capture a frame
            //Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

            using (Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone())
            {
                // Convert to BitmapSource for WPF
                BitmapSource bitmapSource = ConvertToBitmapSource(bitmap);
                // Update the Image control with the new frame
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    camera_img.Source = bitmapSource;
                   

                }), DispatcherPriority.Render);

            }
            // Read QR code
            //Bitmap bitmapimg = (Bitmap)eventArgs.Frame.Clone();
            Bitmap bitmapimg = (Bitmap)eventArgs.Frame.Clone();
            BarcodeReader reader = new BarcodeReader();

            var result = reader.Decode(bitmapimg);
            if (result != null)
            {
                // Do something with the QR code data
                string qrData = result.Text;
                // Invoke on UI thread if necessary
                this.Dispatcher.Invoke(() =>
                {
                    // Update UI with QR code data
                    txt_toview.Text = qrData;
                });
            }


        }
        private BitmapSource ConvertToBitmapSource(Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Bmp);
                memoryStream.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Freeze to make it cross-thread accessible
                return bitmapImage;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            videoSource.Start();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
            base.OnClosing(e);
        }
    }
}
