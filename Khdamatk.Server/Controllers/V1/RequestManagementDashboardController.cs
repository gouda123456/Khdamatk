using Microsoft.AspNetCore.Mvc;
using Khdamatk.Server.Services.Interfaces;
using Khdamatk.Server.Contracts.Admin.Request;

namespace Khdamatk.Server.Controllers.Admin;

[Route("api/admin/requests")]
[ApiController]
public class RequestManagementDashboardController(IRequestManagementDashboardSerivce requestManagementDashboardService) : ControllerBase
{
    private readonly IRequestManagementDashboardSerivce _requestManagementDashboardService = requestManagementDashboardService;

    [HttpGet("list")]
    public async Task<IActionResult> GetOrders([FromQuery] string? status, CancellationToken cancellationToken)
    {
        var result = await _requestManagementDashboardService.GetOrdersAsync(status, cancellationToken);
        return Ok(result);
    }

    [HttpGet("analytics")]
    public async Task<IActionResult> GetOrderAnalytics(CancellationToken cancellationToken)
    {
        var result = await _requestManagementDashboardService.GetOrderAnalyticsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateOrder([FromForm] UpdateOrderAdminRequest request, CancellationToken cancellationToken)
    {
        var result = await _requestManagementDashboardService.UpdateOrderAsync(request, cancellationToken);
        return Ok(result);
    }
}
