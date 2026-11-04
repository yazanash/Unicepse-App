using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Subscription;
using Uniceps.Server.Requests;
using Uniceps.Server.Responses;
using Uniceps.Server.Services;

namespace Uniceps.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IPushPullDataService<Subscription> _dataService;

        public SubscriptionsController(IPushPullDataService<Subscription> dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<IActionResult> Pull(long lastServerVersion = 0)
        {
            try
            {
                IEnumerable<Subscription> subscriptions = await _dataService.PullDataAsync(lastServerVersion);

                return Ok(subscriptions.Select(x => new SubscriptionResponse(x)).ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> Push(List<SubscriptionRequest> subscriptionRequests)
        {
            try
            {
                List<Subscription> subscriptions = subscriptionRequests.Select(x => x.ToModel()).ToList();
                await _dataService.PushDataAsync(subscriptions);
                return Ok("Created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
