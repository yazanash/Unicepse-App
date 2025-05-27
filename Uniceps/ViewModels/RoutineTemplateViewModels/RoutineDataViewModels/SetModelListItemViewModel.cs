using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.utlis.common;
using Uniceps.Commands.RoutineSystemCommands.DayGroupCommands;
using Uniceps.Commands.RoutineSystemCommands.SetModelsCommands;

namespace Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels
{
    public class SetModelListItemViewModel : ViewModelBase
    {
        private readonly SetsModelDataStore _setsModelDataStore;
        public SetModel? SetModel { get; set; }
        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set { _isEditing = value; OnPropertyChanged(nameof(IsEditing)); }
        }

        public ICommand? SubmitCommand { get; }
        public SetModelListItemViewModel(SetModel setModel, SetsModelDataStore setsModelDataStore)
        {
            _setsModelDataStore = setsModelDataStore;

            SetModel = setModel;
            Repetition = setModel.Repetition!;
            RoundIndex = setModel.RoundIndex!;
            SubmitCommand = new UpdateSetModelCommand(_setsModelDataStore, this);
        }
        private int _repetition;
        public int Repetition
        {
            get { return _repetition; }
            set { _repetition = value; OnPropertyChanged(nameof(Repetition)); }
        }
        private int _roundIndex;
        public int RoundIndex
        {
            get { return _roundIndex; }
            set { _roundIndex = value; OnPropertyChanged(nameof(RoundIndex)); }
        }

        internal void Update(SetModel obj)
        {
            SetModel = obj;
            Repetition = obj.Repetition!;
            RoundIndex = obj.RoundIndex!;
            IsEditing = false;
            OnPropertyChanged(nameof(Repetition));
        }
    }
}
