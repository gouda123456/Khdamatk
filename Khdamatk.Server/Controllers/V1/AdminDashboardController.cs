using Khdamatk.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class AdminDashboardController(IAdminDashboardSerivce _statsService) : ControllerBase
{
    ///  High-level system statistics for the admin home screen. 
    [Authorize(Roles = RolesStrings.Admin)]
    [HttpGet("admin")]
    public async Task<IActionResult> GetAdminStats(CancellationToken ct)
        => (await _statsService.GetAdminStats(ct)).Respond();
}