﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.ViewModels;

namespace Unicepse.Commands.RoutinesCommand
{
    internal class LoadAllTempRoutineCommand : AsyncCommandBase
    {
        private readonly RoutineDataStore _routineDataStore;
        private readonly ListingViewModelBase _routineListing;
        private readonly PlayersDataStore _playerDataStore;

        public LoadAllTempRoutineCommand(RoutineDataStore routineDataStore, ListingViewModelBase routineListing, PlayersDataStore playerDataStore)
        {
            _routineDataStore = routineDataStore;
            _routineListing = routineListing;
            _playerDataStore = playerDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _routineListing.ErrorMessage = null;
            _routineListing.IsLoading = true;

            try
            {

                await _routineDataStore.GetAllTemp();
            }
            catch (Exception)
            {
                _routineListing.ErrorMessage = "خطأ في تحميل القوالب يرجى اعادة تشغيل البرنامج";
            }
            finally
            {
                _routineListing.IsLoading = false;
            }
        }
    }
}
