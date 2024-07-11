﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Unicepse.Views.AuthView
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void email_MouseDown(object sender, MouseButtonEventArgs e)
        {
            email_txt.Focus();
        }

        private void password_MouseDown(object sender, MouseButtonEventArgs e)
        {
            password_txt.Focus();
        }

        private void password_txt_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(password_txt.Password) && password_txt.Password.Length > 0)
            {
                lbl_password.Visibility = Visibility.Hidden;
                if (this.DataContext != null)
                { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password; }
            }
            else
            {
                lbl_password.Visibility = Visibility.Visible;
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
                // Simulate button click
                login_btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

                // Optionally, make the button look pressed (visual effect)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic)?
                    .Invoke(login_btn, new object[] { true });

                e.Handled = true; // Suppress the Enter key
            }
        }
    }
}
