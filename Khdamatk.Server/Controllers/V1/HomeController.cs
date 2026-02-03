using Khdamatk.Server.Contracts.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController(IHomeService homeService) : ControllerBase
    {
        private readonly IHomeService homeService = homeService;

        [HttpGet]
        public async Task<IActionResult> HomeMain(HomeMainResponse homeMain, CancellationToken cancellationToken)
        {
            var result = await homeService.HomeMain();
            return result.Respond();
        }
    }
}
