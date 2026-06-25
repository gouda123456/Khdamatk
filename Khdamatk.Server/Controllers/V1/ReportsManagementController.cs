using Khdamatk.Server.Contracts.Dashboard;
using Khdamatk.Server.Contracts.Reports;
using Khdamatk.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _reportService;

        public ReportsController(IReportsService reportService) => _reportService = reportService;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _reportService.GetAllReportsAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReportRequest report)
        {
            await _reportService.AddReportAsync(report);
            return Ok(report);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _reportService.DeleteReportAsync(id);
            return NoContent();
        }
    }
}