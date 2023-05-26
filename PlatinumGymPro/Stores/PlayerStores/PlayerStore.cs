﻿using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Services.PlayerQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores.PlayerStores
{

   public enum PlayerFilter
    {
        Active,NotActive,Trainer
    }
    public class PlayerStore
    {
        private readonly PlayerDataService<Player> PlayerService;
        private readonly List<Player> _players;
        public IEnumerable<Player> Players => _players;


        public event Action? PlayersLoaded;
        public event Action<Player>? PlayerAdded;
        public event Action<Player>? PlayerUpdated;
        public event Action<int>? PlayerDeleted;
        public PlayerStore(PlayerDataService<Player> playerService)
        {
            PlayerService = playerService;
            _players = new List<Player>();
        }
        public async Task Load()
        {
            IEnumerable<Player> players = await PlayerService.GetAll();

            _players.Clear();
            _players.AddRange(players);

            PlayersLoaded?.Invoke();
        }
        public async Task Load(bool status)
        {
            IEnumerable<Player> players = await PlayerService.GetByStatus(status);

            _players.Clear();
            _players.AddRange(players);

            PlayersLoaded?.Invoke();
        }

        public async Task Add(Player player)
        {
            await PlayerService.Create(player);

            _players.Add(player);

            PlayerAdded?.Invoke(player);
        }
        public async Task Update(Player player)
        {
            await PlayerService.Update(player);

            int currentIndex = _players.FindIndex(y => y.Id == player.Id);

            if (currentIndex != -1)
            {
                _players[currentIndex] = player;
            }
            else
            {
                _players.Add(player);
            }

            PlayerUpdated?.Invoke(player);
        }
        public async Task Delete(int id)
        {
            await PlayerService.Delete(id);

            _players.RemoveAll(y => y.Id == id);

            PlayerDeleted?.Invoke(id);
        }
    }
}
