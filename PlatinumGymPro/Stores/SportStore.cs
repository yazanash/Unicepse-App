using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores
{
    public class SportStore
    {
        private readonly SportServices SportService;
        private readonly List<Sport> _sports;
        public IEnumerable<Sport> Sports => _sports;

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
                SelectedSportChanged?.Invoke();
            }
        }
        public event Action? SportLoaded;
        public event Action<Sport>? SportAdded;
        public event Action<Sport>? SportUpdated;
        public event Action? SelectedSportChanged;
        public event Action<int>? SportDeleted;
        public SportStore(SportServices sportService )
        {
            SportService = sportService;
            _sports = new List<Sport>();
        }

        public async Task Load()
        {
            IEnumerable<Sport> sports = await SportService.GetAll();

            _sports.Clear();
            _sports.AddRange(sports);

            SportLoaded?.Invoke();
        }
        public async Task Add(Sport sport,List<Employee> trainers)
        {
            await SportService.Create(sport, trainers);
            _sports.Add(sport);

            SportAdded?.Invoke(sport);
        }
        public async Task Update(Sport sport)
        {
            await SportService.Update(sport);

            int currentIndex = _sports.FindIndex(y => y.Id == sport.Id);

            if (currentIndex != -1)
            {
                _sports[currentIndex] = sport;
            }
            else
            {
                _sports.Add(sport);
            }

            SportUpdated?.Invoke(sport);
        }
        public async Task Delete(int id)
        {
            await SportService.Delete(id);

            _sports.RemoveAll(y => y.Id == id);

            SportDeleted?.Invoke(id);
        }
    }
}
