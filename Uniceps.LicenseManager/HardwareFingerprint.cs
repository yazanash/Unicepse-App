using DeviceId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.LicenseManager
{
    public static class HardwareFingerprint
    {
        public static string GetId()
        {
            return new DeviceIdBuilder()
                .AddMachineName()
                .AddOsVersion()
                .ToString()
                .Substring(0, 20)
                .ToUpper();
        }
    }
}
