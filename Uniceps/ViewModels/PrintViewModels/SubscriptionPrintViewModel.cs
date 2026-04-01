using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Uniceps.Stores;
using Uniceps.ViewModels.PaymentsViewModels;
using Uniceps.Core.Models.Subscription;
using System.IO;
using Uniceps.SystemServices;

namespace Uniceps.ViewModels.PrintViewModels
{
    public class SubscriptionPrintViewModel : ViewModelBase
    {
        public Subscription Subscription;
        private readonly ObservableCollection<PaymentListItemViewModel> _paymentListItemViewModels;
        public IEnumerable<PaymentListItemViewModel> PaymentsList => _paymentListItemViewModels;
        public SubscriptionPrintViewModel(Subscription subscription)
        {
            Subscription = subscription;

            _paymentListItemViewModels = new ObservableCollection<PaymentListItemViewModel>();

            foreach (var item in Subscription.Payments!)
            {
                _paymentListItemViewModels.Add(new PaymentListItemViewModel(item));
            }
            GymName = SettingsManager.Current.GymName;
            LoadProfileImage(SettingsManager.Current.LogoPath);
        }
        private void LoadProfileImage(string? localPath)
        {
            if (!File.Exists(localPath))
                return;

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(localPath);
            bitmap.EndInit();
            bitmap.Freeze();
            GymLogo = bitmap;
        }
        private string? _gymName;
        public string? GymName
        {
            get { return _gymName; }
            set { _gymName = value; OnPropertyChanged(nameof(GymName)); }
        }
        private BitmapImage? _gymLogo;

        public BitmapImage? GymLogo
        {
            get { return _gymLogo; }
            set { _gymLogo = value; OnPropertyChanged(nameof(GymLogo)); }
        }
        public int Id => Subscription.Id;
        public string? PlayerName => Subscription.PlayerName;
        public string? Code => Subscription.Code;
        public string? SportName => Subscription.SportName;
        public int SubDays => Subscription.DaysCount;
        public DateTime LastCheck => Subscription.LastCheck;
        public string? Trainer =>  Subscription.TrainerName?? "بدون مدرب";
        public string RollDate => Subscription.RollDate.ToString("ddd,MMM dd,yyy");
        public double Price => Subscription.Price;
        public double OfferValue => Subscription.OfferValue;
        public string? OfferDes => Subscription.OfferDes;
        public double PriceAfterOffer => Subscription.PriceAfterOffer;
        public string EndDate => Subscription.EndDate.ToString("ddd,MMM dd,yyy");
    }
}
