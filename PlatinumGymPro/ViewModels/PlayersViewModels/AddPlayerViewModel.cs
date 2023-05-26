using PlatinumGymPro.Commands;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.Stores.PlayerStores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.PlayersViewModels
{
    public class AddPlayerViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly PlayerStore _playerStore;
        public AddPlayerViewModel(NavigationStore navigationStore, PlayerStore playerStore,PlayerListViewModel playerListViewModel)
        {
            _navigationStore = navigationStore;
            _playerStore = playerStore;
            CancelCommand = new NavaigateCommand<PlayerListViewModel>(new NavigationService<PlayerListViewModel>(_navigationStore, ()=> playerListViewModel));
            this.SubmitCommand = new SubmitCommand(new NavigationService<PlayerListViewModel>(_navigationStore, () => playerListViewModel), _playerStore,this);
            PropertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
        }

      
        public int Id { get; }

        private string? _fullName;
        public string? FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }
        private string? _phone;
        public string? Phone
        {
            get { return _phone; }
            set
            {
                _phone = value; OnPropertyChanged(nameof(Phone));
                ClearError(nameof(Phone));
                if (Phone?.Trim().Length < 10)
                {
                    AddError("يجب ان يكون رقم الهاتف 10 ارقام", nameof(Phone));
                    OnErrorChanged(nameof(Phone));
                }

            }
        }

        private void AddError(string? ErrorMsg, string? propertyName)
        {
            if (!PropertyNameToErrorsDictionary.ContainsKey(propertyName))
            {
                PropertyNameToErrorsDictionary.Add(propertyName, new List<string>());

            }
            PropertyNameToErrorsDictionary[propertyName].Add(ErrorMsg);
            OnErrorChanged(propertyName);
        }

        private void ClearError(string? propertyName)
        {
            PropertyNameToErrorsDictionary.Remove(propertyName);
            OnErrorChanged(propertyName);
        }

        private void OnErrorChanged(string? PropertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(PropertyName));
        }

        private int _birthDate;
        public int BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value; OnPropertyChanged(nameof(BirthDate)); }
        }
        private bool _genderMale;
        public bool GenderMale
        {
            get { return _genderMale; }
            set { _genderMale = value; OnPropertyChanged(nameof(GenderMale)); }
        }
        private double _weight;
        public double Weight
        {
            get { return _weight; }
            set { _weight = value; OnPropertyChanged(nameof(Weight)); }
        }
        private double _hieght;
        public double Hieght
        {
            get { return _hieght; }
            set { _hieght = value; OnPropertyChanged(nameof(Hieght)); }
        }
        private DateTime _subscribeDate = DateTime.Now.Date;
        public DateTime SubscribeDate
        {
            get { return _subscribeDate; }
            set { _subscribeDate = value; OnPropertyChanged(nameof(SubscribeDate)); }
        }
        private string? _subscribeEndDate = DateTime.Now.Date.ToShortDateString();
        public string? SubscribeEndDate
        {
            get { return _subscribeEndDate; }
            set { _subscribeEndDate = value; OnPropertyChanged(nameof(SubscribeEndDate)); }
        }
        private bool _isTakenContainer;
        public bool IsTakenContainer
        {
            get { return _isTakenContainer; }
            set { _isTakenContainer = value; OnPropertyChanged(nameof(IsTakenContainer)); }
        }
        private bool _isSubscribed;
        public bool IsSubscribed
        {
            get { return _isSubscribed; }
            set { _isSubscribed = value; OnPropertyChanged(nameof(IsSubscribed)); }
        }
        private double _balance;
        public double Balance
        {
            get { return _balance; }
            set { _balance = value; OnPropertyChanged(nameof(Balance)); }
        }
        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }

        public readonly Dictionary<string, List<string>> PropertyNameToErrorsDictionary;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => PropertyNameToErrorsDictionary.Any();

        public IEnumerable? GetErrors(string? propertyName)
        {
            return PropertyNameToErrorsDictionary.GetValueOrDefault(propertyName, new List<string>());
        }
    }
}
