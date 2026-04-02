using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.LicenseManager.Models;

namespace Uniceps.Stores
{
    public class LicenseStore
    {
        public LicenseStatus Current { get; private set; } = LicenseStatus.DefaultTrial();

        public event Action? LicenseChanged;

        public void Update(LicenseStatus status)
        {
            Current = status;
            LicenseChanged?.Invoke();
        }
    }
}
