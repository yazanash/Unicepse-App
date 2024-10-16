using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Commands;
using Unicepse.Commands.RoutinesCommand;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.Stores;
using Unicepse.navigation.Stores;
using Unicepse.navigation;

namespace Unicepse.ViewModels.RoutineViewModels
{
    public class RoutineTemplatesViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<RoutinItemViewModel> _routineItemViewModels;
        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly NavigationStore _navigationStore;
        private readonly RoutinePlayerViewModels _routinePlayerViewModels;
        private readonly LicenseDataStore _licenseDataStore;
        public IEnumerable<RoutinItemViewModel> RoutineList => _routineItemViewModels;
        public ICommand LoadAllRoutines { get; }
        public ICommand AddRoutineCommand { get; }
        public RoutinItemViewModel? SelectedRoutine
        {
            get
            {
                return RoutineList
                    .FirstOrDefault(y => y?.playerRoutine == _routineDataStore.SelectedRoutine);
            }
            set
            {
                _routineDataStore.SelectedRoutine = value?.playerRoutine;
                OnPropertyChanged(nameof(SelectedRoutine));

            }
        }

        public RoutineTemplatesViewModel(RoutineDataStore routineDataStore, PlayersDataStore playersDataStore, NavigationStore navigationStore, RoutinePlayerViewModels routinePlayerViewModels, LicenseDataStore licenseDataStore)
        {
            _routineDataStore = routineDataStore;
            _playersDataStore = playersDataStore;
            _navigationStore = navigationStore;
            _routinePlayerViewModels = routinePlayerViewModels;
            _licenseDataStore = licenseDataStore;
            _routineItemViewModels = new ObservableCollection<RoutinItemViewModel>();

            _routineDataStore.TempLoaded += _routineDataStore_Loaded;
            LoadAllRoutines = new LoadAllTempRoutineCommand(_routineDataStore, this, _playersDataStore);
            //SelectedRoutine = RoutineList.FirstOrDefault();

            AddRoutineCommand = new NavaigateCommand<AddRoutineViewModel>(new NavigationService<AddRoutineViewModel>(_navigationStore, () => LoadAddRoutineViewModel(_playersDataStore, _routineDataStore, new NavigationService<RoutinePlayerViewModels>(_navigationStore, () => _routinePlayerViewModels), _navigationStore, this,_licenseDataStore)));
            ChooseCommand = new NavaigateCommand<AddRoutineViewModel>(new NavigationService<AddRoutineViewModel>(_navigationStore, () => LoadChoosedAddRoutineViewModel(_playersDataStore, _routineDataStore, new NavigationService<RoutinePlayerViewModels>(_navigationStore, () => _routinePlayerViewModels), _navigationStore, true, this,_licenseDataStore)));
            CancelCommand = new NavaigateCommand<RoutinePlayerViewModels>(new NavigationService<RoutinePlayerViewModels>(_navigationStore, () => routinePlayerViewModels));
           
        }
        public ICommand ChooseCommand { get; }
        public ICommand CancelCommand { get; }
        private AddRoutineViewModel LoadChoosedAddRoutineViewModel(PlayersDataStore playersDataStore, RoutineDataStore routineDataStore, NavigationService<RoutinePlayerViewModels> navigationService, NavigationStore navigationStore, bool FromTemp, RoutineTemplatesViewModel routineTemplatesViewModel,LicenseDataStore licenseDataStore)
        {
            return AddRoutineViewModel.LoadViewModel(playersDataStore, routineDataStore, navigationService, navigationStore, FromTemp, routineTemplatesViewModel,licenseDataStore);
        }


        private AddRoutineViewModel LoadAddRoutineViewModel(PlayersDataStore playerStore, RoutineDataStore routineDataStore, NavigationService<RoutinePlayerViewModels> navigationService, NavigationStore navigationStore, RoutineTemplatesViewModel routineTemplatesViewModel,LicenseDataStore licenseDataStore)
        {
            return AddRoutineViewModel.LoadViewModel(playerStore, routineDataStore, navigationService, navigationStore,routineTemplatesViewModel,licenseDataStore);
        }


        private void _routineDataStore_Loaded()
        {
            if (!_routineDataStore.TempRoutines.Any())
                AddRoutineCommand.Execute(null);
            else
            {
                _routineItemViewModels.Clear();
                foreach (var routine in _routineDataStore.TempRoutines.OrderByDescending(x => x.RoutineData))
                {
                    AddRoutine(routine);

                }
                SelectedRoutine = _routineItemViewModels.FirstOrDefault();
            }
            
        }



        public override void Dispose()
        {
            _routineDataStore.Loaded -= _routineDataStore_Loaded;
            base.Dispose();
        }
        private void AddRoutine(PlayerRoutine routine)
        {
            RoutinItemViewModel viewmodel = new(routine, _playersDataStore, _routineDataStore, _navigationStore, _routinePlayerViewModels,_licenseDataStore);
            _routineItemViewModels.Add(viewmodel);
        }

        public static RoutineTemplatesViewModel LoadViewModel(RoutineDataStore routineDataStore, PlayersDataStore playersDataStore, NavigationStore navigationStore, RoutinePlayerViewModels routinePlayerViewModels,LicenseDataStore licenseDataStore)
        {
            RoutineTemplatesViewModel viewModel = new(routineDataStore, playersDataStore, navigationStore, routinePlayerViewModels,licenseDataStore);

            viewModel.LoadAllRoutines.Execute(null);
            return viewModel;
        }
    }
}
