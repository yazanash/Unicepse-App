using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores
{
    public class SelectedPlayerStore
    {
        private Player? _selectedPlayer;

        public Player SelectedPlayer
        {
            get { return _selectedPlayer!; }
            set { 
                _selectedPlayer = value;
                SelectedPlayerChanged?.Invoke();
            }
        }

        public event Action? SelectedPlayerChanged;
    }
}
