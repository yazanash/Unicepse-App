using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.LicenseManager
{
    [SupportedOSPlatform("windows")]
    public static class HardwareFingerprint
    {
        public static string GetId()
        {
            try
            {
                string rawIdentifier =
                    GetProcessorId() +
                    GetBiosSerialNumber() +
                    GetBaseBoardId();

                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawIdentifier));

                    string base64Hash = Convert.ToBase64String(hashBytes)
                        .Replace("/", "")
                        .Replace("+", "")
                        .Replace("=", "");

                    return base64Hash.Substring(0, 20).ToUpper();
                }
            }
            catch (Exception)
            {
                return "UNKNOWN-DEVICE-ID";
            }
        }

        private static string GetProcessorId()
        {
            string id = "";
            using (var searcher = new ManagementObjectSearcher("Select ProcessorId From Win32_Processor"))
            {
                foreach (var obj in searcher.Get())
                    id += obj["ProcessorId"]?.ToString();
            }
            return string.IsNullOrEmpty(id) ? "CPU000" : id;
        }

        private static string GetBiosSerialNumber()
        {
            string id = "";
            using (var searcher = new ManagementObjectSearcher("Select SerialNumber From Win32_BIOS"))
            {
                foreach (var obj in searcher.Get())
                    id += obj["SerialNumber"]?.ToString();
            }
            return string.IsNullOrEmpty(id) ? "BIOS000" : id;
        }

        private static string GetBaseBoardId()
        {
            string id = "";
            using (var searcher = new ManagementObjectSearcher("Select SerialNumber From Win32_BaseBoard"))
            {
                foreach (var obj in searcher.Get())
                    id += obj["SerialNumber"]?.ToString();
            }
            return string.IsNullOrEmpty(id) ? "MB000" : id;
        }
    }
}
