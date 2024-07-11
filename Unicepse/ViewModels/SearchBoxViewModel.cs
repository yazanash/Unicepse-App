﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels
{
    public class SearchBoxViewModel : ViewModelBase
    {
        private string? _searchText;
        public string? SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                SearchedText?.Invoke(SearchText);
            }
        }
        public event Action<string?>? SearchedText;
    }
}
