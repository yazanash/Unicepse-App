using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Uniceps.Helpers
{
    public class ThemeService
    {
        private const string LightThemePath = "Resources/Theme/LightTheme.xaml";
        private const string DarkThemePath = "Resources/Theme/DarkTheme.xaml";
        public static void ApplyTheme(AppTheme theme)
        {
            var dicts = Application.Current.Resources.MergedDictionaries;

            // احذف الثيم القديم
            var oldTheme = dicts.FirstOrDefault(d =>
                d.Source != null &&
                (d.Source.OriginalString.Contains("LightTheme.xaml") ||
                 d.Source.OriginalString.Contains("DarkTheme.xaml")));

            int index = 0;
            if (oldTheme != null)
            {
                index = dicts.IndexOf(oldTheme);
                dicts.RemoveAt(index);
            }

            // أضف الثيم الجديد
            var themePath = theme == AppTheme.Light ? LightThemePath : DarkThemePath;

            dicts.Insert(index, new ResourceDictionary
            {
                Source = new Uri(themePath, UriKind.Relative)
            });
            Properties.Settings.Default.AppTheme = theme.ToString();
            Properties.Settings.Default.Save();

        }

    }
    public enum AppTheme
    {
        Light,
        Dark
    }
}
