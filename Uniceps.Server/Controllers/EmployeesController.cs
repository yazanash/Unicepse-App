using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Player;
using Uniceps.Server.Requests;
using Uniceps.Server.Responses;
using Uniceps.Server.Services;

namespace Uniceps.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IPushPullDataService<Employee> _dataService;

        public EmployeesController(IPushPullDataService<Employee> dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<IActionResult> Pull(long lastServerVersion = 0)
        {
            try
            {
                IEnumerable<Employee> employees = await _dataService.PullDataAsync(lastServerVersion);

                return Ok(employees.Select(x => new EmployeeResponse(x)).ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Push(List<EmployeeRequest> createPlayerRequest)
        {
            try
            {
                List<Employee> employees = createPlayerRequest.Select(x => x.ToModel()).ToList();
                await _dataService.PushDataAsync(employees);
                return Ok("Created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
