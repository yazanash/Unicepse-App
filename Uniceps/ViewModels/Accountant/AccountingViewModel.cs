using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.Expenses;
using Uniceps.ViewModels.Metrics;
using Uniceps.ViewModels.OtherRevenueViewModels;
using Uniceps.ViewModels.PlayersAttendenceViewModels;

namespace Uniceps.ViewModels.Accountant
{
    public class AccountingViewModel : ViewModelBase
    {
        private readonly DailyReportStore _dailyReportStore;
        private readonly LicenseStore _licenseStore;

        // الـ ViewModels الخاصة بالتابات (تُحقن عبر الـ DI بكل نظافة)
        public AccountingStateViewModel AccountingStateViewModel { get; }
        public ExpensesListViewModel ExpensesListViewModel { get; }
        public RevenueListViewModel RevenueListViewModel { get; }
        public ExpensesReportViewModel ExpensesReportViewModel { get; }
        public IncomeReportViewModel IncomeReportViewModel { get; }
        public MounthlyReportViewModel MounthlyReportViewModel { get; }
        public PremiumViewModel PremiumViewModel { get; }

        private int _selectedTabIndex = 0;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
                LoadDataForActiveTab(); // تحميل بيانات التبويب النشط فوراً عند الانتقال إليه
            }
        }

        private bool _isFullVersion;
        public bool IsFullVersion
        {
            get => _isFullVersion;
            set { _isFullVersion = value; OnPropertyChanged(nameof(IsFullVersion)); }
        }

        public AccountingViewModel(
            DailyReportStore dailyReportStore,
            LicenseStore licenseStore,
            AccountingStateViewModel accountingStateViewModel,
            ExpensesListViewModel expensesListViewModel,
            RevenueListViewModel revenueListViewModel,
            ExpensesReportViewModel expensesReportViewModel,
            IncomeReportViewModel incomeReportViewModel,
            MounthlyReportViewModel mounthlyReportViewModel,
            PremiumViewModel premiumViewModel)
        {
            _dailyReportStore = dailyReportStore;
            _licenseStore = licenseStore;

            AccountingStateViewModel = accountingStateViewModel;
            ExpensesListViewModel = expensesListViewModel;
            RevenueListViewModel = revenueListViewModel;
            ExpensesReportViewModel = expensesReportViewModel;
            IncomeReportViewModel = incomeReportViewModel;
            MounthlyReportViewModel = mounthlyReportViewModel;
            PremiumViewModel = premiumViewModel;

            IsFullVersion = _licenseStore.Current.IsFullVersion;

            // التحميل الأولي للتاب المفتوح حالياً
            LoadDataForActiveTab();
        }

        private void LoadDataForActiveTab()
        {
            // منطق تحميل البيانات للتاب الحالي فقط (Lazy Loading) لضمان سرعة الأداء
            switch (_selectedTabIndex)
            {
                case 0:
                    // مثال: تحميل بيانات الحركة اليومية إذا لزم الأمر
                    break;
                case 1:
                    // مثال: تحميل بيانات المصاريف
                    break;
                    // وهكذا بقية التابات...
            }
        }

        public void ResetSelection()
        {
            SelectedTabIndex = IsFullVersion ? 0 : 1;
        }
    }
}
