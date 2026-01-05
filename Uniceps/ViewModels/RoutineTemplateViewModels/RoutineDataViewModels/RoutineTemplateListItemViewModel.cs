
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.navigation.Stores;
using Uniceps.ViewModels.RoutineTemplateViewModels;
using Uniceps.Commands.Player;
using Uniceps.navigation;

namespace Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels
{
    public class RoutineTemplateListItemViewModel : ViewModelBase
    {
        public RoutineModel RoutineModel;
        public ICommand? OpenProfileCommand { get; }
        private readonly NavigationStore? _navigationStore;
        private readonly RoutineDetailsViewModel _routineDetailsViewModel;


        public RoutineTemplateListItemViewModel(RoutineModel routineModel, NavigationStore navigationStore, RoutineDetailsViewModel routineDetailsViewModel)
        {
            RoutineModel = routineModel;
            _navigationStore = navigationStore;
            _routineDetailsViewModel = routineDetailsViewModel;

            OpenProfileCommand = new NavaigateCommand<RoutineDetailsViewModel>(new NavigationService<RoutineDetailsViewModel>(_navigationStore, () => _routineDetailsViewModel));

        }
        public int Id => RoutineModel.Id;
        private int _order = 0;
        public int Order {
            get { return _order; }
            set {  _order = value; OnPropertyChanged(nameof(Order)); } 
        }  
        public string? Name => RoutineModel.Name;
        public RoutineLevel Level => RoutineModel.Level;
        internal void Update(RoutineModel obj)
        {
            RoutineModel = obj;
        }
    }
}
