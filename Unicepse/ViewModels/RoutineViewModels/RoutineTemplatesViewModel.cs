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

        public RoutineTemplatesViewModel(RoutineDataStore routineDataStore, PlayersDataStore playersDataStore, NavigationStore navigationStore, RoutinePlayerViewModels routinePlayerViewModels)
        {
            _routineDataStore = routineDataStore;
            _playersDataStore = playersDataStore;
            _navigationStore = navigationStore;
            _routinePlayerViewModels = routinePlayerViewModels;

            _routineItemViewModels = new ObservableCollection<RoutinItemViewModel>();

            _routineDataStore.Loaded += _routineDataStore_Loaded;
            LoadAllRoutines = new LoadAllTempRoutineCommand(_routineDataStore, this, _playersDataStore);
            //SelectedRoutine = RoutineList.FirstOrDefault();

            AddRoutineCommand = new NavaigateCommand<AddRoutineViewModel>(new NavigationService<AddRoutineViewModel>(_navigationStore, () => LoadAddRoutineViewModel(_playersDataStore, _routineDataStore, new NavigationService<RoutinePlayerViewModels>(_navigationStore, () => _routinePlayerViewModels), _navigationStore)));
            ChooseCommand = new NavaigateCommand<AddRoutineViewModel>(new NavigationService<AddRoutineViewModel>(_navigationStore, () => LoadChoosedAddRoutineViewModel(_playersDataStore, _routineDataStore, new NavigationService<RoutinePlayerViewModels>(_navigationStore, () => _routinePlayerViewModels), _navigationStore, true)));

        }
        public ICommand ChooseCommand { get; }
        private AddRoutineViewModel LoadChoosedAddRoutineViewModel(PlayersDataStore playersDataStore, RoutineDataStore routineDataStore, NavigationService<RoutinePlayerViewModels> navigationService, NavigationStore navigationStore, bool FromTemp)
        {
            return AddRoutineViewModel.LoadViewModel(playersDataStore, routineDataStore, navigationService, navigationStore, FromTemp);
        }


        private AddRoutineViewModel LoadAddRoutineViewModel(PlayersDataStore playerStore, RoutineDataStore routineDataStore, NavigationService<RoutinePlayerViewModels> navigationService, NavigationStore navigationStore)
        {
            return AddRoutineViewModel.LoadViewModel(playerStore, routineDataStore, navigationService, navigationStore);
        }


        private void _routineDataStore_Loaded()
        {
            _routineItemViewModels.Clear();
            foreach (var routine in _routineDataStore.Routines.OrderByDescending(x => x.RoutineData))
            {
                AddRoutine(routine);

            }
            SelectedRoutine = _routineItemViewModels.SingleOrDefault();
        }




        private void AddRoutine(PlayerRoutine routine)
        {
            RoutinItemViewModel viewmodel = new(routine, _playersDataStore, _routineDataStore, _navigationStore, _routinePlayerViewModels);
            _routineItemViewModels.Add(viewmodel);
        }

        public static RoutineTemplatesViewModel LoadViewModel(RoutineDataStore routineDataStore, PlayersDataStore playersDataStore, NavigationStore navigationStore, RoutinePlayerViewModels routinePlayerViewModels)
        {
            RoutineTemplatesViewModel viewModel = new(routineDataStore, playersDataStore, navigationStore, routinePlayerViewModels);

            viewModel.LoadAllRoutines.Execute(null);

            return viewModel;
        }
    }
}
