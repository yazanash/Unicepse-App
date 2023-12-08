using PlatinumGymPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.State.Navigator
{
    public enum ViewType
    {
        Home,
        Players,
        Expenses,
        Accounting,
        Sport,
        Trainer,
    }
    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }

        //ICommand UpdateCurrentViewModelCommand { get; }
     }
}
