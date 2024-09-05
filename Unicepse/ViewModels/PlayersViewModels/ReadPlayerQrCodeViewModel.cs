using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels.PlayersViewModels
{
    public class ReadPlayerQrCodeViewModel : ViewModelBase
    {
        private string? _uid;
        public string? UID 
        { 
            get { return _uid; }
            set { _uid = value; OnPropertyChanged(nameof(UID)); }
        }
    }
}
