using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Services;
using Uniceps.Server.Requests;
using Uniceps.Server.Responses;
using Uniceps.Server.Services;

namespace Uniceps.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPushPullDataService<Player> _dataService;

        public PlayersController(IPushPullDataService<Player> dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<IActionResult> Pull(long lastServerVersion = 0)
        {
            try
            {
                IEnumerable<Player> players = await _dataService.PullDataAsync(lastServerVersion);

                return Ok(players.Select(x => new PlayerResponse(x)).ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Push(List<CreatePlayerRequest> createPlayerRequest)
        {
            try
            {
                List<Player> players = createPlayerRequest.Select(x=>x.ToModel()).ToList();
                await _dataService.PushDataAsync(players);
                return Ok("Created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
