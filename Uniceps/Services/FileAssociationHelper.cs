using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Services
{
    public static class FileAssociationHelper
    {
        public static void RegisterFileAssociation()
        {
            try
            {
                string extension = ".unxlic";
                string programId = "Uniceps.LicenseFile";
                string description = "Uniceps License File";
                string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName??"";

                using (var key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(extension))
                {
                    key.SetValue("", programId);
                }

                using (var key = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(programId))
                {
                    key.SetValue("", description);
                    using (var shellKey = key.CreateSubKey(@"shell\open\command"))
                    {
                        shellKey.SetValue("", $"\"{exePath}\" \"%1\"");
                    }
                }
            }
            catch { /* قد يفشل إذا لم يكن هناك صلاحيات مسؤول */ }
        }
    }
}
