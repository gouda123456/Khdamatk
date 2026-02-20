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
}
