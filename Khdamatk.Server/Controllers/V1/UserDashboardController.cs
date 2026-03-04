using Khdamatk.Server.Contracts.Dashboard;
using Khdamatk.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class UserDashboardController(IUserDashboardService _userService) : ControllerBase
{
    ///  GET User Management Table Data with search/filters. 
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(
        [FromQuery] string? search,
        [FromQuery] string? role,
        [FromQuery] string? status,
        CancellationToken ct)
    {
        var result = await _userService.GetUsersList(search, role, status, ct);
        return result.Respond();
    }

    ///  Update user role (e.g., Freelancer, Admin). 
    [HttpPatch("set-role")]
    public async Task<IActionResult> SetRole([FromBody] UpdateRoleRequest req)
        => (await _userService.SetUserRole(req)).Respond();

    ///  Update user status (e.g., Block, Verify). 
    [HttpPut("user-status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateUserStatusRequest req)
        => (await _userService.UpdateUserStatus(req)).Respond();
}