using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Services
{
    public static class DeviceInfoService
    {
        public static readonly string DeviceToken;
        public static readonly string DeviceId;
        public static readonly string Platform;
        public static readonly string AppVersion;
        public static readonly string DeviceModel;
        public static readonly string OsVersion;

        static DeviceInfoService()
        {
            AppVersion = GetAppVersion();
            string guid = GetMachineGuid();
            DeviceToken = (guid.Length >= 8 ? guid.Substring(0, 8) : guid).ToUpper() + "-WPF";
            DeviceId = "WIN-" + guid.ToUpper();
            Platform = GetPlatform();
            DeviceModel = GetRealDeviceModel();

            OsVersion = Environment.OSVersion.Version.Major.ToString();
        }

        private static string GetAppVersion()
        {
            try
            {
                return Assembly.GetExecutingAssembly().GetName().Version!.ToString();
            }
            catch
            {
            }
            return "1.0.0.0";
        }
        private static string GetPlatform()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return "Windows";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return "macOS";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return "Linux";
            return "Unknown";
        }
        private static string GetRealDeviceModel()
        {
            try
            {
                string manufacturer = "";
                string model = "";

                using (var searcher = new ManagementObjectSearcher("SELECT Manufacturer, Model FROM Win32_ComputerSystem"))
                using (var collection = searcher.Get())
                {
                    foreach (var obj in collection)
                    {
                        manufacturer = obj["Manufacturer"]?.ToString()?.Trim() ?? "";
                        model = obj["Model"]?.ToString()?.Trim() ?? "";
                    }
                }

                string fullModel = $"{manufacturer} {model}".Trim();
                return string.IsNullOrWhiteSpace(fullModel) ? Environment.MachineName : fullModel;
            }
            catch
            {
                return Environment.MachineName;
            }
        }

        private static string GetMachineGuid()
        {
            try
            {
                const string location = @"SOFTWARE\Microsoft\Cryptography";
                const string name = "MachineGuid";

                using var localMachineX64View = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                using var rk = localMachineX64View.OpenSubKey(location);

                if (rk != null)
                {
                    var value = rk.GetValue(name);
                    if (value != null)
                    {
                        return value.ToString() ?? Guid.NewGuid().ToString();
                    }
                }
            }
            catch { }
            return Guid.NewGuid().ToString();
        }
    }
}
