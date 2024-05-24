//using PlatinumGymPro.Models;
using Unicepse.Core.Models.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Unicepse.WPF.ViewModels.PlayersViewModels.vm
{
    public class PlayerViewModel
    {
        private readonly Player _player;

        public int Id => _player.Id;
        public string? FullName => _player.FullName;
        public string? Phone => _player.Phone;
        public int BirthDate => _player.BirthDate;
        public bool GenderMale => _player.GenderMale;
        public double Weight => _player.Weight;
        public double Hieght => _player.Hieght;
        public string? SubscribeDate => _player.SubscribeDate.ToShortDateString();
        public string? SubscribeEndDate => _player.SubscribeEndDate.ToShortDateString();
        public bool IsTakenContainer => _player.IsTakenContainer;
        public bool IsSubscribed => _player.IsSubscribed;
        public double Balance => _player.Balance;


        public ICommand? EditCommand { get; }
        public ICommand? DeleteCommand { get; }

        public PlayerViewModel(Player player)
        {
            _player = player;
        }
    }
}
