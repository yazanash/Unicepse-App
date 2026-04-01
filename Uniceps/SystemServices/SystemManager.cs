using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Helpers;
using Uniceps.Services;

namespace Uniceps.SystemServices
{
    public static class SystemManager
    {
        private static Mutex? mutex = null;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        public static void CheckOpenedApplication()
        {
            const string appName = "Unicepse";
            bool createdNew;
            mutex = new Mutex(true, appName, out createdNew);
            if (!createdNew)
            {
                BringExistingInstanceToFront();
                Application.Current.Shutdown();
            }
        }
        private static void BringExistingInstanceToFront()
        {
            var currentProcess = Process.GetCurrentProcess();
            foreach (var process in Process.GetProcessesByName(currentProcess.ProcessName))
            {
                if (process.Id != currentProcess.Id)
                {
                    SetForegroundWindow(process.MainWindowHandle);
                    break;
                }
            }
        }
        public static void ApplyTheme()
        {
            var savedTheme = SettingsManager.Current.AppTheme;
            if (Enum.TryParse(savedTheme, out AppTheme theme))
            {
                ThemeService.ApplyTheme(theme);
            }
        }
        public static void InitializeSystem()
        {
            CheckOpenedApplication();
            FileAssociationHelper.RegisterFileAssociation();
            ApplyTheme();
        }
    }
}
