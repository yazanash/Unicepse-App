using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Stores
{
    public class BackgroundServiceStore
    {
        public event Action<string?,bool>? StateChanged;

        public void ChangeState(string message , bool connectionStatus)
        {
            StateChanged?.Invoke(message, connectionStatus);
        }
    }
}
