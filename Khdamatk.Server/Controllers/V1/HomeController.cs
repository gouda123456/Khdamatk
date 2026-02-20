using Khdamatk.Server.Contracts.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class HomeController(IHomeService homeService) : ControllerBase
{
    private readonly IHomeService homeService = homeService;

    [HttpGet("")]
    public async Task<IActionResult> GetHomeData(CancellationToken cancellationToken)
    {
        var result = await homeService.MainPage(cancellationToken);
        return result.Respond();
    }

    [HttpGet("Freelancers")]
    public async Task<IActionResult> GetFreelancers([FromQuery] FreelancerRequest freelancerRequest,CancellationToken cancellationToken)
    {
        var result = await homeService.FreelancersPage(freelancerRequest,cancellationToken);
        return result.Respond();
    }
    
}