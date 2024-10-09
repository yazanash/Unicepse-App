using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Commands.RoutinesCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Stores;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels.RoutineViewModels
{
    public class RoutineItemFillViewModel : ViewModelBase
    {
        public RoutineItems RoutineItems;
        private readonly PlayersDataStore _playerStore;
        private readonly RoutineDataStore _routineDataStore;
        public RoutineItemFillViewModel(RoutineItems routineItems, PlayersDataStore playerStore, RoutineDataStore routineDataStore)
        {
            _playerStore = playerStore;
            _routineDataStore = routineDataStore;
            RoutineItems = routineItems;
            _notes = routineItems.Notes;
            _orders = routineItems.Orders;
            _itemOrder = RoutineItems.ItemOrder;
            RemoveItemCommand = new RemoveRoutineItemCommand(_playerStore, _routineDataStore, RoutineItems);
            RepeateItemCommand = new RepeateItemCommand(_playerStore, _routineDataStore, RoutineItems);
            ApplyToAllCommand = new ApplyToAllCommand(_routineDataStore, RoutineItems);
        }
        public ICommand RemoveItemCommand { get; }
        public ICommand RepeateItemCommand { get; }

        public ICommand ApplyToAllCommand { get; }

        public int _itemOrder;
        public int ItemOrder
        {
            get { return _itemOrder; }
            set
            {
                _itemOrder = value;
                OnPropertyChanged(nameof(ItemOrder));
                RoutineItems.ItemOrder = ItemOrder;
            }
        }
        public string? ExerciseName => RoutineItems.Exercises!.Name;
        public string? imageId => "pack://application:,,,/Resources/Assets/Exercises/" + RoutineItems.Exercises!.GroupId + "/" + RoutineItems.Exercises!.ImageId + ".jpg";
        public int GroupId => RoutineItems.Exercises!.GroupId;

        private string? _notes;
        public string? Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                OnPropertyChanged(nameof(Notes));
                RoutineItems.Notes = Notes;
            }
        }
        private string? _orders;
        public string? Orders
        {
            get { return _orders; }
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
                RoutineItems.Orders = Orders;
            }
        }
    }
}
