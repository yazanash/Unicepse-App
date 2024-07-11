﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using emp = Unicepse.Core.Models.Employee;
using Unicepse.ViewModels.Employee.TrainersViewModels;
using Unicepse.Stores;
using Unicepse.navigation;

namespace Unicepse.Commands.Employee
{
    public class EditTrainerCommand : AsyncCommandBase
    {
        private readonly NavigationService<TrainersListViewModel> navigationService;
        private readonly EmployeeStore _employeeStore;
        private EditTrainerViewModel _editTrainerViewModel;

        public EditTrainerCommand(NavigationService<TrainersListViewModel> navigationService, EditTrainerViewModel editTrainerViewModel, EmployeeStore employeeStore)
        {
            this.navigationService = navigationService;
            _employeeStore = employeeStore;
            _editTrainerViewModel = editTrainerViewModel;
            _editTrainerViewModel.PropertyChanged += _editTrainerViewModel_PropertyChanged;
        }

        private void _editTrainerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_editTrainerViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _editTrainerViewModel.CanSubmit && !string.IsNullOrEmpty(_editTrainerViewModel.FullName) && _editTrainerViewModel.Phone!.Trim().Length > 9 && base.CanExecute(null);

        }
        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                emp.Employee employee = new emp.Employee()
                {
                    Id = _employeeStore.SelectedEmployee!.Id,
                    FullName = _editTrainerViewModel.FullName,
                    BirthDate = _editTrainerViewModel.Year!.year,
                    GenderMale = _editTrainerViewModel.GenderMale,
                    IsActive = true,
                    IsTrainer = true,
                    IsSecrtaria = false,
                    ParcentValue = _editTrainerViewModel.ParcentValue,
                    SalaryValue = _editTrainerViewModel.SalaryValue,
                    Phone = _editTrainerViewModel.Phone,
                    StartDate = _editTrainerViewModel.StartDate,
                    Position = _editTrainerViewModel.Position,
                };
                await _employeeStore.DeleteConnectedSports(employee.Id);
                foreach (var SportListItem in _editTrainerViewModel.SportList)
                {
                    if (SportListItem.IsSelected)
                        employee.Sports!.Add(SportListItem.sport);
                }
                await _employeeStore.Update(employee);
                navigationService.ReNavigate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }

}
