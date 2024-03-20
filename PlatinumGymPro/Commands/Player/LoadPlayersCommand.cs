//using PlatinumGymPro.Models;
using PlatinumGymPro.Stores;
using PlatinumGymPro.Stores.PlayerStores;
using PlatinumGymPro.ViewModels;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands
{
    public class LoadPlayersCommand : AsyncCommandBase
    {
        private readonly PlayersDataStore _playerStore;
        private readonly ListingViewModelBase _playerListing;


        public LoadPlayersCommand(ListingViewModelBase playerListing, PlayersDataStore playerStore)
        {
            _playerStore = playerStore;
            _playerListing = playerListing;
            _playerStore = playerStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _playerListing.ErrorMessage = null;
            _playerListing.IsLoading = true;

            try
            {
                #region filtering
                //switch (_playerStore.SelectedFilters?.Id)
                //{
                //    case 1:
                //        await _playerStore.LoadByGender(true);
                //        break;
                //    case 2:
                //        await _playerStore.LoadByGender(false);
                //        break;
                //    case 3:
                //        await _playerStore.Load();
                //        break;
                //    case 4:
                //        await _playerStore.Load(false);
                //        break;
                //    case 5:
                //        await _playerStore.Load();
                //        break;
                //    case 6:
                //        await _playerStore.Load(true);
                //        break;
                //    case 7:
                //        await _playerStore.LoadBySubscribeEnd();
                //        break;
                //    case 8:
                //        await _playerStore.LoadByDebt();
                //        break;
                //    default:
                //        await _playerStore.Load(true);
                //        break;
                //}
                #endregion

                await _playerStore.GetPlayers();
            }
            catch (Exception)
            {
                _playerListing.ErrorMessage = "Failed to load Players. Please restart the application.";
            }
            finally
            {
                _playerListing.IsLoading = false;
            }
        }
    }
}
