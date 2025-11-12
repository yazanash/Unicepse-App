using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace Uniceps.Views
{
    /// <summary>
    /// Interaction logic for SplashScreenWindow.xaml
    /// </summary>
    public partial class SplashScreenWindow : Window
    {
        private static readonly string currentVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        public SplashScreenWindow()
        {
            InitializeComponent();
            lbl_Version.Text = currentVersion;
        }
    }
}
