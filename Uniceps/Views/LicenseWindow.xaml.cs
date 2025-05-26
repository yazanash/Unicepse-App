using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Uniceps.Views
{
    /// <summary>
    /// Interaction logic for LicenseWindow.xaml
    /// </summary>
    public partial class LicenseWindow : Window
    {
        public LicenseWindow()
        {
            InitializeComponent();
        }
        private void email_MouseDown(object sender, MouseButtonEventArgs e)
        {
            email_txt.Focus();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();

            }
        }


        private void email_txt_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(email_txt.Text) && email_txt.Text.Length > 0)
            {
                lbl_username.Visibility = Visibility.Hidden;

            }
            else
            {
                lbl_username.Visibility = Visibility.Visible;
            }
        }
        private void Minimize_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Close_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();



        }
        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {

                var command = ((Button)verify_btn).Command;
                if (command != null && command.CanExecute(null))
                {
                    command.Execute(null);
                }
            }

        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

    }
}
