﻿using Unicepse.Core.Models;
using Unicepse.Core.Models.Sport;
using Unicepse.Commands;
using Unicepse.Commands.Employee;
using Unicepse.Commands.SubscriptionCommand;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using Unicepse.Stores;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.Employee.TrainersViewModels
{
    public class EditTrainerViewModel : ErrorNotifyViewModelBase
    {
        private readonly ObservableCollection<SportTrainerListItemViewModel> _sportListItemViewModels;
        private readonly SportDataStore _sportDataStore;
        private readonly NavigationStore _navigationStore;
        private readonly TrainersListViewModel _trinersListViewModel;
        private readonly EmployeeStore _employeeStore;
        public IEnumerable<SportTrainerListItemViewModel> SportList => _sportListItemViewModels;
        public ObservableCollection<Year> years;

        public IEnumerable<Year> Years => years;
        public EditTrainerViewModel(NavigationStore navigationStore, TrainersListViewModel trinersListViewModel, SportDataStore sportDataStore, EmployeeStore employeeStore)
        {
            years = new ObservableCollection<Year>();
            for (int i = DateTime.Now.Year - 80; i < DateTime.Now.Year; i++)
                years.Add(new Year() { year = i });
            Year = years.SingleOrDefault(x => x.year == DateTime.Now.Year - 1);
            _sportDataStore = sportDataStore;
            _navigationStore = navigationStore;
            _trinersListViewModel = trinersListViewModel;
            _employeeStore = employeeStore;
            _sportListItemViewModels = new();
            _sportDataStore.Loaded += _sportDataStore_Loaded;
            CancelCommand = new NavaigateCommand<TrainersListViewModel>(new NavigationService<TrainersListViewModel>(_navigationStore, () => _trinersListViewModel));
            SubmitCommand = new EditTrainerCommand(new NavigationService<TrainersListViewModel>(_navigationStore, () => _trinersListViewModel), this, _employeeStore);
            LoadSportsCommand = new LoadSportItemsCommand(_sportDataStore);
            FullName = _employeeStore.SelectedEmployee!.FullName;
            Phone = _employeeStore.SelectedEmployee!.Phone;
            Year = years.SingleOrDefault(x => x.year == _employeeStore.SelectedEmployee!.BirthDate);
            GenderMale = _employeeStore.SelectedEmployee!.GenderMale;
            SalaryValue = _employeeStore.SelectedEmployee!.SalaryValue;
            ParcentValue = _employeeStore.SelectedEmployee!.ParcentValue;
            Position = _employeeStore.SelectedEmployee!.Position;
            StartDate = _employeeStore.SelectedEmployee!.StartDate;
        }

        private void _sportDataStore_Loaded()
        {
            _sportListItemViewModels.Clear();
            foreach (var sport in _sportDataStore.Sports)
            {
                AddSport(sport);
            }

            foreach (var t in _employeeStore.SelectedEmployee!.Sports!)
            {
                if (SportList.FirstOrDefault(x => x.sport.Id == t.Id) != null)
                    SportList.FirstOrDefault(x => x.sport.Id == t.Id)!.IsSelected = true;
            }
        }

        private void AddSport(Sport sport)
        {
            SportTrainerListItemViewModel sportListItemViewModel = new(sport);
            _sportListItemViewModels.Add(sportListItemViewModel);
        }

        private string? _fullName;
        public string? FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
                ClearError(nameof(FullName));
                if (string.IsNullOrEmpty(FullName?.Trim()))
                {
                    AddError("هذا الحقل مطلوب", nameof(FullName));
                    OnErrorChanged(nameof(FullName));
                }
            }
        }

        private string? _phone = "0";
        public string? Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
                ClearError(nameof(Phone));
                if (Phone?.Trim().Length < 10)
                {
                    AddError("يجب ان يكون رقم الهاتف 10 ارقام", nameof(Phone));
                    OnErrorChanged(nameof(Phone));
                }
            }
        }

        private Year? _year;
        public Year? Year
        {
            get { return _year; }
            set
            {
                _year = value;
                OnPropertyChanged(nameof(Year));
            }
        }

        private bool _genderMale;
        public bool GenderMale
        {
            get { return _genderMale; }
            set
            {
                _genderMale = value;
                OnPropertyChanged(nameof(GenderMale));
            }
        }

        private double _salaryValue;
        public double SalaryValue
        {
            get { return _salaryValue; }
            set
            {
                _salaryValue = value;
                OnPropertyChanged(nameof(SalaryValue));
            }
        }

        private int _parcentValue;
        public int ParcentValue
        {
            get { return _parcentValue; }
            set
            {
                _parcentValue = value;
                OnPropertyChanged(nameof(ParcentValue));
                ClearError(nameof(ParcentValue));
                if (ParcentValue > 100)
                {
                    AddError("لا يمكن ان تكون النسبة اكبر من 100%", nameof(ParcentValue));
                    OnErrorChanged(nameof(ParcentValue));
                }
                else if (ParcentValue < 0)
                {
                    AddError("لايمكن ان تكون القيمة اقل من 0", nameof(ParcentValue));
                    OnErrorChanged(nameof(ParcentValue));
                }
            }
        }

        private string? _position;
        public string? Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        private DateTime _startDate = DateTime.Now;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
        public ICommand LoadSportsCommand { get; }

        public static EditTrainerViewModel LoadViewModel(NavigationStore navigatorStore, TrainersListViewModel trainersListViewModel, SportDataStore sportDataStore, EmployeeStore employeeStore)
        {
            EditTrainerViewModel viewModel = new EditTrainerViewModel(navigatorStore, trainersListViewModel, sportDataStore, employeeStore);

            viewModel.LoadSportsCommand.Execute(null);

            return viewModel;
        }
    }

}
