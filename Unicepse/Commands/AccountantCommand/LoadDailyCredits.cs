﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.ViewModels.Accountant;

namespace Unicepse.Commands.AccountantCommand
{
    public class LoadDailyCredits : AsyncCommandBase
    {
        private readonly GymStore _gymStore;
        private readonly AccountingStateViewModel _accountingStateViewModel;
        public LoadDailyCredits(GymStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            _gymStore = gymStore;
            _accountingStateViewModel = accountingStateViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _gymStore.GetDailyCredits(_accountingStateViewModel.Date);
        }
    }
}
