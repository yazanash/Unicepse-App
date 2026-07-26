using System;
using System.Windows.Input;
using Uniceps.Commands.Employee;
using Uniceps.Stores;
using Emp = Uniceps.Core.Models.Employee;
namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class AddEmployeeViewModel : ErrorNotifyViewModelBase
    {
        private readonly EmployeeStore _employeeStore;
        public Emp.Employee? Employee;
        public bool IsEditMode = false;
        public AddEmployeeViewModel(EmployeeStore employeeStore)
        {
            _employeeStore = employeeStore;
            SubmitCommand = new AddEmployeeCommand(this, _employeeStore);
        }
        public AddEmployeeViewModel(EmployeeStore employeeStore, Emp.Employee employee)
        {
            _employeeStore = employeeStore;
            SubmitCommand = new AddEmployeeCommand(this, _employeeStore);
            Employee = employee;
            IsEditMode = true;
            FullName = Employee.FullName;
            Phone = Employee.Phone;
            Year = Employee.BirthDate;
            GenderMale = Employee.GenderMale;
            SalaryValue = Employee.SalaryValue;
            ParcentValue = Employee.ParcentValue;
            Position = Employee.Position;
            StartDate = Employee.StartDate;

        }

        public Action? EmployeeCreated;
        public void OnEmployeeCreated()
        {
            EmployeeCreated?.Invoke();
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

        private int _year = DateTime.Now.Year;
        public int Year
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
        public void ClearForm()
        {
            FullName = "";
            Phone = "";
        }
        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
    }
}
