using System.Windows.Media;
using Uniceps.BackgroundServices;
using Uniceps.Core.Common;
using Uniceps.Stores;
using Uniceps.ViewModels;

namespace Uniceps.utlis.ComponentsViewModels
{
    public class StatusBarViewModel : ViewModelBase
    {
        private readonly AccountStore _accountStore;
        private readonly BackgroundServiceStore _backgroundServiceStore;
        public StatusBarViewModel(AccountStore accountStore, BackgroundServiceStore backgroundServiceStore)
        {
            _accountStore = accountStore;
            _accountStore.StateChanged += _accountStore_StateChanged;
            _backgroundServiceStore = backgroundServiceStore;
            _backgroundServiceStore.StateChanged += _backgroundServiceStore_StateChanged;
            _backgroundServiceStore.SyncStatus += _backgroundServiceStore_SyncStatus;
        }

        private void _accountStore_StateChanged()
        {
            OnPropertyChanged(nameof(UserName));
            OnPropertyChanged(nameof(Role));
            OnPropertyChanged(nameof(Position));
            OnPropertyChanged(nameof(OwnerName));
        }
        private void _backgroundServiceStore_SyncStatus(bool obj, string? message)
        {
            OnPropertyChanged(nameof(SyncState));
            OnPropertyChanged(nameof(SyncMessage));

        }
        private void _backgroundServiceStore_StateChanged(string? obj, bool connectionStatus)
        {
            OnPropertyChanged(nameof(BackMessage));
            OnPropertyChanged(nameof(Connection));
        }

        public string? UserName => _accountStore.CurrentAccount?.UserName;
        public Roles? Role => _accountStore.CurrentAccount!?.Role;
        public string? Position => _accountStore.CurrentAccount?.Position;
        public string? OwnerName => _accountStore.CurrentAccount?.OwnerName;

        public string? BackMessage => _backgroundServiceStore.BackMessage;
        public Brush? Connection => _backgroundServiceStore.Connection ? Brushes.Green : Brushes.Red;
        public bool SyncState => _backgroundServiceStore.SyncStateProp;
        public string? SyncMessage => _backgroundServiceStore.SyncMessage;
    }
}
