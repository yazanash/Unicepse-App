using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.ViewModels
{
    public class SplashScreenViewModel:ViewModelBase
    {
        private string? _message;
        public string? Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(nameof(Message)); }
        }
        private double _progress;
        public double Progress
        {
            get => _progress;
            set { _progress = value; OnPropertyChanged(nameof(Progress));
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        public bool IsLoading => Progress == 0;
    }
}
