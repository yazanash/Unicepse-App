using PlatinumGym.Core.Models.Employee;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using emp = PlatinumGym.Core.Models.Employee;

namespace PlatinumGymPro.ViewModels.TrainersViewModels
{
    public class TrainerListItemViewModel : ViewModelBase 
    {
        public emp.Employee Trainer;
        //privatemp.e readonly TrainerStore _trainerStore;
        private readonly NavigationStore _navigationStore;
        public int Id => Trainer.Id;
        public string? FullName => Trainer.FullName;
        public double SalaryValue => Trainer.SalaryValue;
        public double ParcentValue => Trainer.ParcentValue;
        public string? Phone => Trainer.Phone;
        public int BirthDate => Trainer.BirthDate;
        public string? Position => Trainer.Position;
        
        public ICommand? EditCommand { get; }
        public ICommand? DeleteCommand { get; }

        public TrainerListItemViewModel(emp.Employee trainer,  NavigationStore navigationStore)
        {
            Trainer = trainer;

            //_trainerStore = trainerStore;
            _navigationStore = navigationStore;
        }

        public void Update(emp.Employee trainer)
        {
            this.Trainer = trainer;

            OnPropertyChanged(nameof(trainer));
        }
    }
}
