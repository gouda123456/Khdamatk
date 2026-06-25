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

    [HttpGet("category/{Category}")]
    public async Task<IActionResult> GetCategoryJobAsync([FromRoute] int Category)
    {
        var result = await jobService.GetCategoryJobAsync(Category);
        return Ok(result);
    }

    [HttpGet("user/{UserId}")]
    public async Task<IActionResult> GetUsersJobAsync([FromRoute] string UserId)
    {
        var result = await jobService.GetUsersJobAsync(UserId);
        return Ok(result);
    }
    [HttpGet("AvailableJobs")]
    public async Task<IActionResult> AvailableJobs(CancellationToken ct)
    {
        var result = await jobService.GetJobsAsync();
        return result.Respond();
    }

}


