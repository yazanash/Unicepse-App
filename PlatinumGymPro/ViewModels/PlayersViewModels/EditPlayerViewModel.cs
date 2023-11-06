using PlatinumGym.Core.Models.Player;
//using PlatinumGymPro.Models;
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
    public class EditPlayerViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        //private readonly PlayerStore _playerStore;


        #region Properties

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
            if (!PropertyNameToErrorsDictionary.ContainsKey(propertyName!))
            {
                PropertyNameToErrorsDictionary.Add(propertyName!, new List<string>());

            }
            PropertyNameToErrorsDictionary[propertyName!].Add(ErrorMsg!);
            OnErrorChanged(propertyName);
        }

        private void ClearError(string? propertyName)
        {
            //PropertyNameToErrorsDictionary.Remove(propertyName!);
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
        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }

        #endregion
        public readonly Dictionary<string, List<string>> PropertyNameToErrorsDictionary;

        public EditPlayerViewModel(NavigationStore navigationStore, Player player)
        {
            PropertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
            _navigationStore = navigationStore;
            //_playerStore = playerStore;
            Id = player.Id;
            FullName = player.FullName;
            Phone = player.Phone;
            BirthDate = player.BirthDate;
            GenderMale = player.GenderMale;
            Weight = player.Weight;
            Hieght = player.Hieght;
            SubscribeDate = player.SubscribeDate;
          
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => PropertyNameToErrorsDictionary.Any();

        public IEnumerable? GetErrors(string? propertyName)
        {
            return PropertyNameToErrorsDictionary!.GetValueOrDefault(propertyName, new List<string>());
        }

     
    }
}
