﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using Unicepse.Commands;
using Unicepse.ViewModels.Accountant;
using Unicepse.Stores.AccountantStores;

namespace Unicepse.Commands.AccountantCommand
{
    public class LoadExpensesReportCommand : AsyncCommandBase
    {
        private readonly ExpensesAccountantDataStore _expensesDataStore;
        private readonly ExpensesReportViewModel _expensesReportViewModel;

        public LoadExpensesReportCommand(ExpensesAccountantDataStore expensesDataStore, ExpensesReportViewModel expensesReportViewModel)
        {
            _expensesDataStore = expensesDataStore;
            _expensesReportViewModel = expensesReportViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                await _expensesDataStore.GetAll(_expensesReportViewModel.DateFrom, _expensesReportViewModel.DateTo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
