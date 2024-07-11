﻿using Unicepse.Commands.MetricsCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.Metrics
{
    public class AddMetricsViewModel : ViewModelBase
    {
        private readonly MetricDataStore _metricDataStore;
        private readonly NavigationStore _navigationStore;
        private readonly MetricReportViewModel _metricReportViewModel;
        private readonly PlayersDataStore _playerDataStore;
        public AddMetricsViewModel(MetricDataStore metricDataStore, NavigationStore navigationStore, MetricReportViewModel metricReportViewModel, PlayersDataStore playerDataStore)
        {
            _metricDataStore = metricDataStore;
            _navigationStore = navigationStore;
            _metricReportViewModel = metricReportViewModel;
            _playerDataStore = playerDataStore;
            SubmitCommand = new AddMetricsCommand(_metricDataStore, this, _playerDataStore, new NavigationService<MetricReportViewModel>(_navigationStore, () => _metricReportViewModel));
            CancelCommand = new NavaigateCommand<MetricReportViewModel>(new NavigationService<MetricReportViewModel>(_navigationStore, () => _metricReportViewModel));

        }
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
        #region Properties

        /// Hieght and Wieght 
        public double _hieght;
        public double Hieght
        {
            get { return _hieght; }
            set
            {
                _hieght = value;
                OnPropertyChanged(nameof(Hieght));
            }
        }

        public double _wieght;
        public double Wieght
        {
            get { return _wieght; }
            set
            {
                _wieght = value;
                OnPropertyChanged(nameof(_wieght));
            }
        }
        /// Arm 
        public double _lArm;
        public double L_Arm
        {
            get { return _lArm; }
            set
            {
                _lArm = value;
                OnPropertyChanged(nameof(L_Arm));
            }
        }
        public double _rArm;
        public double R_Arm
        {
            get { return _rArm; }
            set
            {
                _rArm = value;
                OnPropertyChanged(nameof(R_Arm));
                L_Arm = R_Arm;

            }
        }
        /// Hummerus
        public double _lHumerus;
        public double L_Humerus
        {
            get { return _lHumerus; }
            set
            {
                _lHumerus = value;
                OnPropertyChanged(nameof(L_Humerus));
                R_Humerus = L_Humerus;
            }
        }
        public double _rHumerus;
        public double R_Humerus
        {
            get { return _rHumerus; }
            set
            {
                _rHumerus = value;
                OnPropertyChanged(nameof(R_Humerus));
            }
        }
        /// Thigh
        public double _lThigh;
        public double L_Thigh
        {
            get { return _lThigh; }
            set
            {
                _lThigh = value;
                OnPropertyChanged(nameof(L_Thigh));
            }
        }
        public double _rThigh;
        public double R_Thigh
        {
            get { return _rThigh; }
            set
            {
                _rThigh = value;
                OnPropertyChanged(nameof(R_Thigh));
                L_Thigh = R_Thigh;

            }
        }
        /// Leg
        public double _lLeg;
        public double L_Leg
        {
            get { return _lLeg; }
            set
            {
                _lLeg = value;
                OnPropertyChanged(nameof(L_Leg));
            }
        }
        public double _rLeg;
        public double R_Leg
        {
            get { return _rLeg; }
            set
            {
                _rLeg = value;
                OnPropertyChanged(nameof(R_Leg));
                L_Leg = R_Leg;

            }
        }
        /// Neck
        public double _nick;
        public double Nick
        {
            get { return _nick; }
            set
            {
                _nick = value;
                OnPropertyChanged(nameof(Nick));
            }
        }
        /// Shoulders
        public double _shoulders;
        public double Shoulders
        {
            get { return _shoulders; }
            set
            {
                _shoulders = value;
                OnPropertyChanged(nameof(Shoulders));
            }
        }
        /// Waist
        public double _waist;
        public double Waist
        {
            get { return _waist; }
            set
            {
                _waist = value;
                OnPropertyChanged(nameof(Waist));
            }
        }
        /// Chest
        public double _chest;
        public double Chest
        {
            get { return _chest; }
            set
            {
                _chest = value;
                OnPropertyChanged(nameof(Chest));
            }
        }
        /// Hips
        public double _hips;
        public double Hips
        {
            get { return _hips; }
            set
            {
                _hips = value;
                OnPropertyChanged(nameof(Hips));
            }
        }
        /// CheckDate
        public DateTime _checkDate = DateTime.Now;
        public DateTime CheckDate
        {
            get { return _checkDate; }
            set
            {
                _checkDate = value;
                OnPropertyChanged(nameof(CheckDate));
            }
        }

        #endregion
    }
}
