using Asp.Versioning;
using Khdamatk.Server.Contracts.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
[ApiVersion(1)]
public class HomeController(IHomeService homeService,IServiceProviderService serviceProviderService) : ControllerBase
{
    private readonly IHomeService homeService = homeService;
    private readonly IServiceProviderService serviceProviderService = serviceProviderService;


    [HttpGet("")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetHomeData(CancellationToken cancellationToken)
    {
        var result = await homeService.MainPage(cancellationToken);
        return result.Respond();
    }

    [HttpGet("Discover")]
    public async Task<IActionResult> GetDiscoverPage()
    {
        var result = await homeService.GetDiscoverPageAsync();
        return Ok(result);
    }


}