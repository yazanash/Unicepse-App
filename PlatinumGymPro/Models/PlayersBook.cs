using PlatinumGymPro.Exceptions;
using PlatinumGymPro.Services.PlayerConflictValidators;
using PlatinumGymPro.Services.PlayerCreations;
using PlatinumGymPro.Services.PlayersProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Models
{
    public class PlayersBook
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly IPlayerCreator _playerCreation;
        private readonly IPlayerConflictValidator _playerConflictValidator;

        public PlayersBook(IPlayerProvider playerProvider, IPlayerCreator playerCreation, IPlayerConflictValidator playerConflictValidator)
        {
            _playerProvider = playerProvider;
            _playerCreation = playerCreation;
            _playerConflictValidator = playerConflictValidator;
        }

        public async Task<IEnumerable<Player>> GetPlayers()
        {
            return await _playerProvider.GetAllPlayers();
        }
        public async Task AddPlayer(Player player)
        {
            Player ConflictPlayer = await _playerConflictValidator.GetConflicting(player);
            if(ConflictPlayer != null)
            {
                throw new PlayerConflictException(ConflictPlayer, player);
            }
          await _playerCreation.Create(player);
        }
    }
}
