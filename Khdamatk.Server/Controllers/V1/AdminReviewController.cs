using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Khdamatk.Server.Contracts.Admin.Review;
using Khdamatk.Server.Services.Interfaces;

namespace Khdamatk.Server.Controllers.Admin;

//[Authorize(Roles = "Admin")]
[AllowAnonymous] 
[Route("api/admin/reviews")]
[ApiController]
public class AdminReviewController(IAdminReviewService adminReviewService) : ControllerBase
{
    private readonly IAdminReviewService _adminReviewService = adminReviewService;

    [HttpGet("list")]
    public async Task<IActionResult> GetReviews([FromQuery] string? status, CancellationToken cancellationToken)
    {
        var result = await _adminReviewService.GetReviewsAsync(status, cancellationToken);
        return Ok(result);
    }

    [HttpGet("analytics")]
    public async Task<IActionResult> GetReviewAnalytics(CancellationToken cancellationToken)
    {
        var result = await _adminReviewService.GetReviewAnalyticsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost("moderate")]
    public async Task<IActionResult> ModerateReview([FromBody] ModerateReviewRequest request, CancellationToken cancellationToken)
    {
        var result = await _adminReviewService.ModerateReviewAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}