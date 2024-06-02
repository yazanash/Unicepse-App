using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unicepse.Core.Models.TrainingProgram;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.ViewModels.RoutineViewModels;
using Unicepse.Stores;
using Unicepse.Commands;
using Unicepse.navigation;

namespace Unicepse.Commands.RoutinesCommand
{
    public class SubmitRoutineCommand : AsyncCommandBase
    {
        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly NavigationService<RoutinePlayerViewModels> _navigationService;
        private readonly SelectRoutineDaysMuscleGroupViewModel selectRoutineDaysMuscleGroupViewModel;
        public SubmitRoutineCommand(RoutineDataStore routineDataStore, PlayersDataStore playersDataStore, NavigationService<RoutinePlayerViewModels> navigationService, SelectRoutineDaysMuscleGroupViewModel selectRoutineDaysMuscleGroupViewModel)
        {
            _routineDataStore = routineDataStore;
            _playersDataStore = playersDataStore;
            _navigationService = navigationService;
            this.selectRoutineDaysMuscleGroupViewModel = selectRoutineDaysMuscleGroupViewModel;
            this.selectRoutineDaysMuscleGroupViewModel.PropertyChanged += SelectRoutineDaysMuscleGroupViewModel_PropertyChanged;
        }

        private void SelectRoutineDaysMuscleGroupViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(selectRoutineDaysMuscleGroupViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {

            return selectRoutineDaysMuscleGroupViewModel.CanSubmit && !string.IsNullOrEmpty(selectRoutineDaysMuscleGroupViewModel.Number) && base.CanExecute(null);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            PlayerRoutine playerRoutine = new()
            {
                RoutineNo = selectRoutineDaysMuscleGroupViewModel.Number,
                RoutineData = selectRoutineDaysMuscleGroupViewModel.Date,
                Player = _playersDataStore.SelectedPlayer!.Player,
                IsTemplate = selectRoutineDaysMuscleGroupViewModel.IsTemplate
            };

            playerRoutine.RoutineSchedule.AddRange(_routineDataStore.RoutineItems);
            foreach (var item in selectRoutineDaysMuscleGroupViewModel.DayGroupList)
            {
                playerRoutine.DaysGroupMap!.Add(item.SelectedDay, item.Groups);
            }

            await _routineDataStore.Add(playerRoutine);
            //string jsonString = ExportToJsonTemplate(playerRoutine);

            //SaveFileDialog dlg = new SaveFileDialog();
            //dlg.FileName = "player_routine"; // Default file name
            //dlg.DefaultExt = ".json"; // Default file extension
            //dlg.Filter = "JSON files (.json)|*.json"; // Filter files by extension

            //bool? result = dlg.ShowDialog();

            //if (result == true)
            //{
            //    string filename = dlg.FileName;
            //    File.WriteAllText(filename, jsonString);
            //}
            _navigationService.ReNavigate();
        }

        public string ExportToJsonTemplate(PlayerRoutine playerRoutine)
        {
            var customJson = new JObject
            {
                ["RoutineId"] = playerRoutine.Id,
                ["RoutineNumber"] = playerRoutine.RoutineNo,
                ["RoutineDate"] = playerRoutine.RoutineData.ToString("yyyy-MM-dd"),
                ["pid"] = playerRoutine.Player != null ? playerRoutine.Player.Id : null,
                ["Schedule"] = JArray.FromObject(playerRoutine.RoutineSchedule.Select(x => new
                {
                    id = x.Id,
                    ExerciseName = x.Exercises!.Name,
                    ExerciseImage = x.Exercises.ImageId,
                    Muscle_Group = x.Exercises.GroupId,
                    orders = x.Orders,
                    notes = x.Notes,
                    itemOrder = x.ItemOrder
                })),
                ["GroupMapping"] = JObject.FromObject(playerRoutine.DaysGroupMap)
            };
            return customJson.ToString(Formatting.Indented); ;
        }
    }
}
