using System.Diagnostics;

namespace Uniceps.Server.SystemServices
{
    public static class FirewallHelper
    {
        public static void OpenPortInFirewall(int port = 9000)
        {
            try
            {
          
                var processInfo = new ProcessStartInfo("cmd.exe", $"/c netsh advfirewall firewall add rule name=\"UnicepsServerPort\" dir=in action=allow protocol=TCP localport={port}")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    Verb = "runas" 
                };

                Process.Start(processInfo);
            }
            catch
            {
               
            }
        }
    }
}
