using PlatinumGymPro.DbContexts;
using PlatinumGymPro.Exceptions;
using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands
{
    public class MakePlayerCommand : AsyncCommandBase
    {
        private readonly GymStore _gymStore;
        private readonly MakePlayerViewModel _makePlayerViewModel;
        public readonly NavigationService<PlayerListingViewModel> _navigationService;

        public MakePlayerCommand(GymStore gymStore, MakePlayerViewModel makePlayerViewModel,NavigationService<PlayerListingViewModel> navigationService)
        {

            _gymStore = gymStore;
            _makePlayerViewModel = makePlayerViewModel;
            _navigationService = navigationService;
            _makePlayerViewModel.PropertyChanged += MakePlayerViewModel_PropertyChanged;
        }

        private void MakePlayerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           if(e.PropertyName == nameof(MakePlayerViewModel.FullName))
            {
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty (_makePlayerViewModel.FullName) && base.CanExecute(parameter);
        }
       

        public override async Task ExecuteAsync(object? parameter)
        {
            Player player = new()
            {
                FullName = _makePlayerViewModel.FullName,
                Phone = _makePlayerViewModel.Phone,
                Balance = _makePlayerViewModel.Balance,
                BirthDate = _makePlayerViewModel.BirthDate,
                GenderMale = _makePlayerViewModel.GenderMale,
                Hieght = _makePlayerViewModel.Hieght,
                IsSubscribed = _makePlayerViewModel.IsSubscribed,
                IsTakenContainer = _makePlayerViewModel.IsTakenContainer,
                SubscribeDate = _makePlayerViewModel.SubscribeDate,
                SubscribeEndDate = Convert.ToDateTime(_makePlayerViewModel.SubscribeEndDate),
                Weight = _makePlayerViewModel.Weight,
            };
            try
            {

                await _gymStore.MakePlayer(player);
                MessageBox.Show("تم الاضافة بنجاح", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService.Navigate();
            }
            catch (PlayerConflictException)
            {
                MessageBox.Show("هذا الاسم موجود لايمكن التكرار", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("فشل في عملية الاضافة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
