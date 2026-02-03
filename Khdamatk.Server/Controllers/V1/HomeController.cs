using Khdamatk.Server.Contracts.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class HomeController(IHomeService homeService) : ControllerBase
{
    private readonly IHomeService homeService = homeService;

    {
        return result.Respond();
    }
}
}
