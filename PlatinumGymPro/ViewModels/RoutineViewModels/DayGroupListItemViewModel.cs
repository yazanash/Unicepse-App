using PlatinumGymPro.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.RoutineViewModels
{
    public class DayGroupListItemViewModel : ViewModelBase
    {
        public int SelectedDay { get; set; }
        public string SelectedDayString { get; set; }
        public List<int> Groups;
        public DayGroupListItemViewModel(int day, string selectedDayString)
        {
            Groups = new List<int>();
            SelectedDay = day;
            SelectedDayString = selectedDayString;
        }
        private bool _chest;
        public bool Chest 
        {
            get { return _chest; }
            set { _chest = value;
                
                OnPropertyChanged(nameof(Chest));
                if (Chest)
                {
                    Groups.Add(((int)EMuscleGroup.Chest));
                }
                else
                    Groups.Remove(((int)EMuscleGroup.Chest));
            }
        }
        private bool _shoulders;
        public bool Shoulders
        {
            get { return _shoulders; }
            set { _shoulders = value; OnPropertyChanged(nameof(Shoulders));
                if (Shoulders)
                {
                    Groups.Add(((int)EMuscleGroup.Shoulders));
                }
                else
                    Groups.Remove(((int)EMuscleGroup.Shoulders));
            }
        }
        private bool _back;
        public bool Back
        {
            get { return _back; }
            set { _back = value; OnPropertyChanged(nameof(Back));
                if (Back)
                {
                    Groups.Add(((int)EMuscleGroup.Back));
                }
                else
                    Groups.Remove(((int)EMuscleGroup.Back));
            }
        }
        private bool _legs;
        public bool Legs
        {
            get { return _legs; }
            set { _legs = value; OnPropertyChanged(nameof(Legs));
                if (Legs)
                {
                    Groups.Add(((int)EMuscleGroup.Legs));
                }
                else
                    Groups.Remove(((int)EMuscleGroup.Legs));
            }
        }
        private bool _biceps;
        public bool Biceps
        {
            get { return _biceps; }
            set { _biceps = value; OnPropertyChanged(nameof(Biceps));
                if (Biceps)
                {
                    Groups.Add(((int)EMuscleGroup.Biceps));
                }
                else
                    Groups.Remove(((int)EMuscleGroup.Biceps));
            }
        }
        private bool _triceps;
        public bool Triceps
        {
            get { return _triceps; }
            set { _triceps = value; OnPropertyChanged(nameof(Triceps));
                if (Triceps)
                {
                    Groups.Add(((int)EMuscleGroup.Triceps));
                }
                else
                    Groups.Remove(((int)EMuscleGroup.Triceps));
            }
        }
        private bool _abs;
        public bool Abs
        {
            get { return _abs; }
            set { _abs = value; OnPropertyChanged(nameof(Abs));
                if (Abs)
                {
                    Groups.Add(((int)EMuscleGroup.Abs));
                }
                else
                    Groups.Remove(((int)EMuscleGroup.Abs));
            }
        }

    }
}
