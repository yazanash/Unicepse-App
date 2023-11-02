using PlatinumGymPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores
{
    public interface INavigationStore
    {
         ViewModelBase CurrentViewModel { get; }  
        public event Action? CurrentViewModelChanged;
    }
}
