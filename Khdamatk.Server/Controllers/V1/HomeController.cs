using Khdamatk.Server.Contracts.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class HomeController(IHomeService homeService,IServiceProviderService serviceProviderService) : ControllerBase
{
    private readonly IHomeService homeService = homeService;
    private readonly IServiceProviderService serviceProviderService = serviceProviderService;


    [HttpGet("")]
    public async Task<IActionResult> GetHomeData(CancellationToken cancellationToken)
    {
        var result = await homeService.MainPage(cancellationToken);
        return result.Respond();
    }


}