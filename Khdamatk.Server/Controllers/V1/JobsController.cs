using Khdamatk.Server.Contracts.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class JobsController(IJobService jobService) : ControllerBase
{
    private readonly IJobService jobService = jobService;

    [HttpGet]
    public async Task<IActionResult> GetAllJobs()
    {
        var result = await jobService.GetAllJobsAsync();
        return result.Respond();

    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetJob(int Id)
    {
        var result = await jobService.GetJobAsync(Id);
        return result.Respond();
    }

    [HttpGet]
    public async Task<IActionResult> GetJobs([FromQuery] JobsFilterRequest request)
    {
        var result = await jobService.GetJobsAsync(request);
        return Ok(result);
    }
}


