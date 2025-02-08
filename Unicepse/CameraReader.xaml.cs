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
using Unicepse.utlis.common;
using System.Reflection;
using Unicepse.ViewModels.PlayersViewModels;

namespace Unicepse
{
    /// <summary>
    /// Interaction logic for CameraReader.xaml
    /// </summary>
    class Camera : ViewModelBase
    {
        public string Name { get; set; }
        public string MonikerString { get; set; }
        public Camera(string name, string monikerString)
        {
            Name = name;
            MonikerString = monikerString;
        }
    }
    public partial class CameraReader : Window
    {
        FilterInfoCollection videoDevices;
        VideoCaptureDevice? videoSource;
        ReadPlayerQrCodeViewModel? _viewModelBase;
        bool _stillOpen= false;
        public CameraReader()
        {
            InitializeComponent();
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            for (int i = 0; i < videoDevices.Count; i++)
            {
                vidlist.Items.Add(new Camera(videoDevices[i].Name, videoDevices[i].MonikerString));
            }
            vidlist.SelectedIndex = 0;
            //videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            //videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            //videoSource.Start();
        }
        public CameraReader(bool stillOpen, ReadPlayerQrCodeViewModel viewModelBase)
        {
            InitializeComponent();
            _stillOpen=stillOpen;
            _viewModelBase = viewModelBase;
            _viewModelBase.UID = null;
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            for (int i = 0; i < videoDevices.Count; i++)
            {
                vidlist.Items.Add(new Camera(videoDevices[i].Name, videoDevices[i].MonikerString));
            }
            vidlist.SelectedIndex = 0;
            this.Topmost = true;
            //videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            //videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            //videoSource.Start();
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
                this.Dispatcher.InvokeAsync(() =>
                {
                    // Update UI with QR code data
               
                        txt_toview.Text = qrData;
                        PlaySoundFromResources();
                        if(_viewModelBase != null)
                        {
                       
                            RestartCamera();
                        _viewModelBase.OnCatchChanged();
                            
                        }
                        else if (!_stillOpen)
                        {
                            this.Close();
                        }

                 




                });
                // Shutdown the dispatcher
                //Dispatcher.InvokeShutdown();


            }


        }

        private void RestartCamera()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource.NewFrame -= video_NewFrame;
                videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
                videoSource.Start();
            }
        }

        public void PlaySoundFromResources()
        {
            // Get the current assembly
            //var assembly = Assembly.GetExecutingAssembly();
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                // Get the stream for the embedded resource
                Stream? soundStream = assembly.GetManifestResourceStream("Unicepse.Resources.sounds.beep.wav");

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
            videoSource!.Start();
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
        private void CloseWindow()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource.NewFrame -= video_NewFrame;
                videoSource = null;
            }
            this.Close();
        }

        private void vidlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (vidlist.SelectedItem is Camera cam)
            {
                if (videoSource != null && videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                    videoSource.NewFrame -= video_NewFrame;
                }
                videoSource = new VideoCaptureDevice(cam.MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
                videoSource.Start();
            }
        }
    }
}
