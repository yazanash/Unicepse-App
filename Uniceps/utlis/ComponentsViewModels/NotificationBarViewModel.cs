using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Uniceps.ViewModels;

namespace Uniceps.utlis.ComponentsViewModels
{
    public class NotificationBarViewModel : ViewModelBase
    {
        private string? _notification;
        public string? Notification
        {
            get => _notification;
            set {  _notification= value; OnPropertyChanged(nameof(Notification)); }
        }
        private Brush? _notificationBarColor;
        public Brush? NotificationBarColor
        {
            get => _notificationBarColor;
            set { _notificationBarColor = value; OnPropertyChanged(nameof(NotificationBarColor)); }
        }
        private string? _actionTitle;
        public string? ActionTitle
        {
            get => _actionTitle;
            set { _actionTitle=  value; OnPropertyChanged(nameof(ActionTitle)); }
        }
        private ICommand? _notificationCommand;
        public ICommand? NotificationCommand
        {
            get => _notificationCommand;
            set { _notificationCommand = value; OnPropertyChanged(nameof(NotificationCommand)); }
        }
        private bool _hasNotification = false;
        public bool HasNotification
        {
            get => _hasNotification;
            set { _hasNotification = value; OnPropertyChanged(nameof(HasNotification)); }
        }
    }
}
