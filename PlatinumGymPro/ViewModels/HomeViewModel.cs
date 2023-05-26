using PlatinumGymPro.Commands;
using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.HomePageViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public ObservableCollection<PlayerViewModel> Players { get; } 
        public ICommand? AddPlayerCommand { get; }
        public ICommand? LoadPlayersCommand { get; }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }
        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
        private bool _isLoaded;
        public bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                _isLoaded = value;
                OnPropertyChanged(nameof(IsLoaded));
            }
        }
        public HomeViewModel()
        {
            Players = new();
            
          
            Players.Add(new PlayerViewModel(new Player()
            {
                FullName = "banda",
                Balance = 0,
                BirthDate = 2019,
                GenderMale = true,
                Hieght = 150,
                IsSubscribed = false,
                IsTakenContainer = false,
                Phone = "09999999990",
                SubscribeDate = DateTime.Now,
                SubscribeEndDate = DateTime.Now,
                Weight = 30,
                Id = 50
            }
           )
               );
            Players.Add(new PlayerViewModel(new Player()
            {
                FullName = "fish",
                Balance = 0,
                BirthDate = 2019,
                GenderMale = true,
                Hieght = 150,
                IsSubscribed = false,
                IsTakenContainer = false,
                Phone = "09999999990",
                SubscribeDate = DateTime.Now,
                SubscribeEndDate = DateTime.Now,
                Weight = 30,
                Id = 50
            }
           )
               );
            Players.Add(new PlayerViewModel(new Player()
            {
                FullName = "sdfdsf",
                Balance = 0,
                BirthDate = 2019,
                GenderMale = true,
                Hieght = 150,
                IsSubscribed = false,
                IsTakenContainer = false,
                Phone = "09999999990",
                SubscribeDate = DateTime.Now,
                SubscribeEndDate = DateTime.Now,
                Weight = 30,
                Id = 50
            }
           )
               );
            Players.Add(new PlayerViewModel(new Player()
            {
                FullName = "sdfsdf",
                Balance = 0,
                BirthDate = 2019,
                GenderMale = true,
                Hieght = 150,
                IsSubscribed = false,
                IsTakenContainer = false,
                Phone = "09999999990",
                SubscribeDate = DateTime.Now,
                SubscribeEndDate = DateTime.Now,
                Weight = 30,
                Id = 50
            }
           )
               );
        }

     

     
    }
}
