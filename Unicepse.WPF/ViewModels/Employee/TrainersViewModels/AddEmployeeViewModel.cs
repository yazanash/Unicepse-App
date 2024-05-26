using Unicepse.Core.Models;
using Unicepse.WPF.Commands;
using Unicepse.WPF.Commands.Employee;
using Unicepse.WPF.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.WPF.navigation.Stores;
using Unicepse.WPF.Commands.Player;
using Unicepse.WPF.navigation;
namespace Unicepse.WPF.ViewModels.Employee.TrainersViewModels
{
    public class AddEmployeeViewModel : ErrorNotifyViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly TrainersListViewModel _trinersListViewModel;
        private readonly EmployeeStore _employeeStore;
        public ObservableCollection<Year> years;

        public IEnumerable<Year> Years => years;
        public AddEmployeeViewModel(NavigationStore navigationStore, TrainersListViewModel trinersListViewModel, EmployeeStore employeeStore)
        {
            years = new ObservableCollection<Year>();
            for (int i = DateTime.Now.Year - 80; i < DateTime.Now.Year; i++)
                years.Add(new Year() { year = i });
            Year = years.SingleOrDefault(x => x.year == DateTime.Now.Year - 1);
            _navigationStore = navigationStore;
            _trinersListViewModel = trinersListViewModel;
            _employeeStore = employeeStore;
            CancelCommand = new NavaigateCommand<TrainersListViewModel>(new NavigationService<TrainersListViewModel>(_navigationStore, () => _trinersListViewModel));
            SubmitCommand = new AddEmployeeCommand(new NavigationService<TrainersListViewModel>(_navigationStore, () => _trinersListViewModel), this, _employeeStore);
            
           
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

        private string? _phone="0";
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

        private bool _isSecertary;
        public bool IsSecertary
        {
            get { return _isSecertary; }
            set
            {
                _isSecertary = value;
                OnPropertyChanged(nameof(IsSecertary));
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
                ClearError(nameof(SalaryValue));
                if (SalaryValue < 0)
                {
                    AddError("لايمكن ان تكون القيمة اقل من 0", nameof(SalaryValue));
                    OnErrorChanged(nameof(SalaryValue));
                }
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

        private double _balance;
        public double Balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
                OnPropertyChanged(nameof(Balance));
            }
        }

        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
    }
}
