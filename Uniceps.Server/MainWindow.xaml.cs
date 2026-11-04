using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Uniceps.Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DateTime _startTime;
        private readonly DispatcherTimer _uptimeTimer;
        public MainWindow()
        {
            InitializeComponent();
            _startTime = DateTime.Now;

            TxtServerIp.Text = $"http://{GetLocalIpAddress()}:9000";

            _uptimeTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _uptimeTimer.Tick += (s, e) =>
            {
                var elapsed = DateTime.Now - _startTime;
                TxtUptime.Text = $"{elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
            };
            _uptimeTimer.Start();
        }
        private string GetLocalIpAddress()
        {
            try
            {
                foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (netInterface.OperationalStatus == OperationalStatus.Up &&
                        netInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                        netInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel &&
                        !netInterface.Description.ToLower().Contains("virtual") &&
                        !netInterface.Description.ToLower().Contains("vpn") &&
                        !netInterface.Description.ToLower().Contains("vmware") &&
                        !netInterface.Description.ToLower().Contains("hyper-v"))
                    {
                        var ipProps = netInterface.GetIPProperties();
                        foreach (var addr in ipProps.UnicastAddresses)
                        {
                            if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                return addr.Address.ToString();
                            }
                        }
                    }
                }

                return "localhost";
            }
            catch
            {
                return "localhost";
            }
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
        }

        // زر الخروج النهائي
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}