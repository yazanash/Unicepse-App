﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels.PlayersViewModels
{
    public class PlayerStatesViewModel : ViewModelBase
    {
        public string? PlayerState { get; set; }
        private double? _stateValue;
        public double? StateValue
        {
            get
            {
                return _stateValue;
            }
            set
            {
                _stateValue = value;
                OnPropertyChanged(nameof(StateValue));
            }
        }
        public MahApps.Metro.IconPacks.PackIconMaterialKind IconPacks { get; set; }
    }
}
