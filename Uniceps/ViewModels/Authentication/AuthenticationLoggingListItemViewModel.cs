using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Uniceps.Core.Models.Authentication;

namespace Uniceps.ViewModels.Authentication
{
    public class AuthenticationLoggingListItemViewModel : ViewModelBase
    {
        public AuthenticationLog authenticationLog;
        private int _order;
        public int Order
        {
            get { return _order; }
            set { _order = value; OnPropertyChanged(nameof(Order)); }
        }
        public int Id => authenticationLog.Id;
        public string? UserName => authenticationLog.User!.UserName;
        public string OperationDate => authenticationLog.LoginDateTime.ToShortDateString();
        public string OperationTime => authenticationLog.LoginDateTime.ToShortTimeString();
        public string Operation => authenticationLog.status ? "دخول" : "خروج";
        public Brush StatusColor => authenticationLog.status ? Brushes.Green : Brushes.Red;
        public AuthenticationLoggingListItemViewModel(AuthenticationLog authenticationLog)
        {
            this.authenticationLog = authenticationLog;
        }
    }
}
