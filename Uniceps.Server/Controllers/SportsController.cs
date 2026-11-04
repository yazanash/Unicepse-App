using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;
using Uniceps.Server.Requests;
using Uniceps.Server.Responses;
using Uniceps.Server.Services;

namespace Uniceps.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        private readonly IPushPullDataService<Sport> _dataService;

        public SportsController(IPushPullDataService<Sport> dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<IActionResult> Pull(long lastServerVersion = 0)
        {
            try
            {
                IEnumerable<Sport> sports = await _dataService.PullDataAsync(lastServerVersion);

                return Ok(sports.Select(x => new SportResponse(x)).ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Push(List<SportRequest> createPlayerRequest)
        {
            try
            {
                List<Sport> sports = createPlayerRequest.Select(x => x.ToModel()).ToList();
                await _dataService.PushDataAsync(sports);
                return Ok("Created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
