using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.ViewModels.PlayersViewModels
{
    public class ReadPlayerQrCodeViewModel : ViewModelBase
    {
        private string? _uid;
        public string? UID
        {
            get { return _uid; }
            set { _uid = value; OnPropertyChanged(nameof(UID)); }
        }
        public event Action? onCatch;
        public void OnCatchChanged()
        {
            onCatch?.Invoke();
        }
    }
}
