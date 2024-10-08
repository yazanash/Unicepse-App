﻿using Unicepse.Core.Models.Sport;
using Unicepse.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Employee;

namespace Unicepse.Stores
{
    public class SportDataStore : IDataStore<Sport>
    {
        public event Action<Sport>? Created;
        public event Action? Loaded;
        public event Action<Sport>? Updated;
        public event Action<int>? Deleted;
        public event Action<Employee?>? TrainerChanged;
        private readonly SportServices _sportDataService;
        private readonly List<Sport> _sports;
        private readonly List<Employee> _trainers;

        private readonly Lazy<Task> _initializeLazy;

        public IEnumerable<Sport> Sports => _sports;

        public IEnumerable<Employee> Trainers => _trainers;

        public SportDataStore(SportServices sportDataService)
        {
            _sportDataService = sportDataService;
            _sports = new List<Sport>();
            _trainers = new List<Employee>();
            _initializeLazy = new Lazy<Task>(Initialize);
        }
        private Sport? _selectedSport;
        public Sport? SelectedSport
        {
            get
            {
                return _selectedSport;
            }
            set
            {
                _selectedSport = value;
                _trainers.Clear();
                if (SelectedSport != null)
                {
                    _trainers.Add(new Employee() { Id = -1, FullName = "الكل"});
                    _trainers.Add(new Employee() { Id = -2, FullName = "بدون مدرب" });
                    _trainers.AddRange(SelectedSport.Trainers!);

                }
                StateChanged?.Invoke(SelectedSport);
            }
        }
        private Employee? _selectedTrainer;
        public Employee? SelectedTrainer
        {
            get
            {
                return _selectedTrainer;
            }
            set
            {
                _selectedTrainer = value;
                TrainerChanged?.Invoke(SelectedTrainer);
            }
        }
        public event Action<Sport?>? StateChanged;
        public async Task Add(Sport entity)
        {
            await _sportDataService.Create(entity);
            _sports.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Delete(int entity_id)
        {
            bool deleted = await _sportDataService.Delete(entity_id);
            int currentIndex = _sports.FindIndex(y => y.Id == entity_id);
            _sports.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }
        public async Task Delete(Sport entity)
        {
            entity.IsActive = false;
            await _sportDataService.Update(entity);
            int currentIndex = _sports.FindIndex(y => y.Id == entity.Id);
            _sports.RemoveAt(currentIndex);
            Deleted?.Invoke(entity.Id);
        }
        public async Task GetAll()
        {
            await _initializeLazy.Value;
            Loaded?.Invoke();
        }

        public async Task Initialize()
        {
            IEnumerable<Sport> sports = await _sportDataService.GetAll();
            _sports.Clear();
            _sports.AddRange(sports);
        }
        public async Task DeleteConnectedTrainers(int Id)
        {
            await _sportDataService.DeleteConnectedTrainers(Id);

        }
        public async Task Update(Sport entity)
        {
            await _sportDataService.Update(entity);
            int currentIndex = _sports.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _sports[currentIndex] = entity;
            }
            else
            {
                _sports.Add(entity);
            }
            Updated?.Invoke(entity);
        }
    }
}
