using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.navigation.Stores;
using Uniceps.ViewModels;

namespace Uniceps.navigation.Navigator
{
    public enum ViewType
    {
        Home,
        PlayersLog,
        Players,
        Expenses,
        Accounting,
        Sport,
        Trainer,
        Users,
        About,
        Logout,
        Exercises
    }
    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }
        ICommand UpdateCurrentViewModelCommand { get; }
    }
}
