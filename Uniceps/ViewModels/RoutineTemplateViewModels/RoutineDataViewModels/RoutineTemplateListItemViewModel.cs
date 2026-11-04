using System.Windows.Input;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.navigation.NavigationSystem;
using Uniceps.Commands;
using System.Collections.Generic;

namespace Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels
{
    public class RoutineTemplateListItemViewModel : ViewModelBase
    {
        public RoutineModel RoutineModel;
        private readonly INavigationService _navigationService;
        public ICommand? OpenProfileCommand { get; }


        public RoutineTemplateListItemViewModel(RoutineModel routineModel,  INavigationService navigationService)
        {
            RoutineModel = routineModel;
            _navigationService = navigationService;
            var navQuery = new Dictionary<string, object>()
            {
                { "RoutineModel",RoutineModel }
            };
            OpenProfileCommand = new RelayCommand(()=>_navigationService.Navigate<RoutineDetailsViewModel>(navQuery));
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
