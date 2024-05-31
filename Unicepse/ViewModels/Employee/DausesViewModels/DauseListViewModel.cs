using Unicepse.Core.Models.Employee;
using Unicepse.Commands;
using Unicepse.Commands.Employee.DauseCommads;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using Unicepse.ViewModels.Employee.TrainersViewModels;
using Unicepse.ViewModels;
using Unicepse.Stores;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.Employee.DausesViewModels
{
    public class DauseListViewModel : ListingViewModelBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly DausesDataStore _dausesDataStore;
        private readonly NavigationStore _navigatorStore;
        private readonly ObservableCollection<TrainerMounthlyReportViewModel> _trainerDausesViewModels;
        public IEnumerable<TrainerMounthlyReportViewModel> Dauses => _trainerDausesViewModels;
        public DauseListViewModel(EmployeeStore employeeStore, DausesDataStore dausesDataStore, NavigationStore navigatorStore)
        {
            _employeeStore = employeeStore;
            _dausesDataStore = dausesDataStore;
            _navigatorStore = navigatorStore;

            _trainerDausesViewModels = new ObservableCollection<TrainerMounthlyReportViewModel>();
            _dausesDataStore.Loaded += _dausesDataStore_Loaded;
            _dausesDataStore.Created += _dausesDataStore_Created;
            _dausesDataStore.Updated += _dausesDataStore_Updated;
            LoadTrainerDauses = new LoadDausesCommand(_employeeStore, _dausesDataStore);
            CreateTrainerDauses = new NavaigateCommand<DausesDetailsViewModel>(new NavigationService<DausesDetailsViewModel>(_navigatorStore, () => new DausesDetailsViewModel(_employeeStore, _dausesDataStore, _navigatorStore, this)));

        }
        public ICommand LoadTrainerDauses { get; }
        public ICommand CreateTrainerDauses { get; }
        private void _dausesDataStore_Updated(TrainerDueses obj)
        {
            TrainerMounthlyReportViewModel? trainerDause =
                   _trainerDausesViewModels.FirstOrDefault(y => y.trainerDueses.Id == obj.Id);

            if (trainerDause != null)
            {
                trainerDause.Update(obj);
            }
        }

        private void _dausesDataStore_Created(TrainerDueses obj)
        {
            addTrainerDauses(obj);
        }

        private void _dausesDataStore_Loaded()
        {
            _trainerDausesViewModels.Clear();
            foreach (var dause in _dausesDataStore.Dauses)
            {
                addTrainerDauses(dause);
            }
        }
        private void addTrainerDauses(TrainerDueses trainerDueses)
        {
            TrainerMounthlyReportViewModel trainerDause = new TrainerMounthlyReportViewModel(trainerDueses, _employeeStore, _dausesDataStore);
            _trainerDausesViewModels.Add(trainerDause);
        }
        public static DauseListViewModel LoadViewModel(EmployeeStore employeeStore, DausesDataStore dausesDataStore, NavigationStore navigatorStore)
        {
            DauseListViewModel viewModel = new DauseListViewModel(employeeStore, dausesDataStore, navigatorStore);
            viewModel.LoadTrainerDauses.Execute(null);
            return viewModel;
        }
    }
}
