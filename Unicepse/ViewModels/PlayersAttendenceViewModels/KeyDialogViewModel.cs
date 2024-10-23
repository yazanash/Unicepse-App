﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands;
using Unicepse.Stores;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels.PlayersAttendenceViewModels
{
    public class KeyDialogViewModel : ViewModelBase
    {
        public string PlayerName { get; set; }
        public int Key { get; set; }
        public ICommand AddKeyCommand { get; }
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        public KeyDialogViewModel(string playerName, PlayersAttendenceStore playersAttendenceStore)
        {
            _playersAttendenceStore = playersAttendenceStore;
            PlayerName = playerName;
            AddKeyCommand = new AddKeyCommand(_playersAttendenceStore, this);
        }
    }
}
