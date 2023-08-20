using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels
{
    public class AccountingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<DataBin> playerListItemViewModels;
        public IEnumerable<DataBin> PlayerList => playerListItemViewModels;
        public AccountingViewModel()
        {
            playerListItemViewModels = new ObservableCollection<DataBin>();
            playerListItemViewModels.Add(new DataBin("sdf sdf sdf", "sdfsdfsdfsdfsdf", 25));
            playerListItemViewModels.Add(new DataBin("SS FDF", "xcvxcvxc", 65));
            playerListItemViewModels.Add(new DataBin("sdfDSF sdfsdf", "l;kl; kl;kkl", 30));
            playerListItemViewModels.Add(new DataBin("sdfsSDFdfsdf", "m,n,n m,", 45));
            playerListItemViewModels.Add(new DataBin("sdf sSD F dfsdf", "we rwrwerw", 12));
            playerListItemViewModels.Add(new DataBin("sdfSDFs dfsdf", "sdfs dfsf d", 89));
            playerListItemViewModels.Add(new DataBin("sdf sdSD Ffsdf", "zxcz xczx", 435));
            playerListItemViewModels.Add(new DataBin("sdf SDF sdfsdf", "sdfsd fsdfsdfsdf", 453));
            playerListItemViewModels.Add(new DataBin("sd fsdf sdf", "cxbv dbgbf", 123));
            playerListItemViewModels.Add(new DataBin("sd fSDFs dfsdf", "fabe barmbm,afe", 13));
            playerListItemViewModels.Add(new DataBin("sdfs DFdfs df", "sebaes bfnet", 435));
            playerListItemViewModels.Add(new DataBin("sdfsD Sdfsdf", "esFsdvf sdvfwbV", 453));
        }
    }
   public class DataBin
    {
        public DataBin(string name, string description, int count)
        {
            Nme = name;
            Description = description;
            Count = count;
        }

        public string Nme { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
    }
}
