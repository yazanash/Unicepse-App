using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.ExpensesCommands;
using Uniceps.Commands.RevenueCommands;
using Uniceps.Core.Models;
using Uniceps.Stores;

namespace Uniceps.ViewModels.OtherRevenueViewModels
{
    public class AddRevenueViewModel : ErrorNotifyViewModelBase
    {
        private readonly OtherRevenuesDataStore _otherRevenuesDataStore;
        public OtherRevenue? OtherRevenue;
        public bool IsEditMode= false;
        public AddRevenueViewModel(OtherRevenuesDataStore otherRevenuesDataStore)
        {
            _otherRevenuesDataStore = otherRevenuesDataStore;
            SubmitCommand = new SubmitRevenueCommand(_otherRevenuesDataStore, this);

        }
        public AddRevenueViewModel(OtherRevenuesDataStore otherRevenuesDataStore, OtherRevenue otherRevenue)
        {
            _otherRevenuesDataStore = otherRevenuesDataStore;
            SubmitCommand = new SubmitRevenueCommand(_otherRevenuesDataStore, this);
            OtherRevenue = otherRevenue;
            IsEditMode = true;
            Amount = OtherRevenue.Amount;
            CustomerName = OtherRevenue.CustomerName;
            Service = OtherRevenue.Service;
            Description = OtherRevenue.Description;
            Date = OtherRevenue.Date;
        }
        public Action? RevenueCreated;
        public void OnRevenueCreated()
        {
            RevenueCreated?.Invoke();
        }
        public ICommand SubmitCommand { get; }
        #region Properties
        private decimal _amount;
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value; OnPropertyChanged(nameof(Amount));
                ClearError(nameof(Amount));
                if (Amount < 0)
                {
                    AddError("لايمكن الدفع بقيمة اقل من 0", nameof(Amount));
                    OnErrorChanged(nameof(Amount));
                }
            }
        }
        private string? _customerName;
        public string? CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; OnPropertyChanged(nameof(CustomerName)); }
        }
        private string? _service;
        public string? Service
        {
            get { return _service; }
            set { _service = value; OnPropertyChanged(nameof(Service)); }
        }
        private string? _description;
        public string? Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }
        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(nameof(Date)); }
        }
        #endregion
    }
}
