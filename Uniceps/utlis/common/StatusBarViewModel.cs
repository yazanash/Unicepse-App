using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Uniceps.utlis.common
{
    public class StatusBarViewModel : ViewModelBase
    {
        public string? UserName { get; set; }
        public string? Role { get; set; }
        public string? Position { get; set; }
        public string? OwnerName { get; set; }
        public StatusBarViewModel(string? userName, string? position, string? ownerName)
        {
            UserName = userName;
            Position = position;
            OwnerName = ownerName;
        }

        private string? _backMessage;
        public string? BackMessage
        {
            get { return _backMessage; }
            set { _backMessage = value; OnPropertyChanged(nameof(BackMessage)); }
        }
        private Brush? _connection;
        public Brush? Connection
        {
            get { return _connection; }
            set { _connection = value; OnPropertyChanged(nameof(Connection)); }
        }
        private bool _syncState;
        public bool SyncState
        {
            get { return _syncState; }
            set { _syncState = value; OnPropertyChanged(nameof(SyncState)); }
        }
        private string? _syncMessage;
        public string? SyncMessage
        {
            get { return _syncMessage; }
            set { _syncMessage = value; OnPropertyChanged(nameof(SyncMessage)); }
        }
    }
}
