using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Khdamatk.Server.Contracts.Admin.Review;
using Khdamatk.Server.Services.Interfaces;

namespace Khdamatk.Server.Controllers.Admin;

//[Authorize(Roles = "Admin")]
[AllowAnonymous] // هنشيلها ونحط [Authorize(Roles = "Admin")] بعد التيست الكامل
[Route("api/admin/reviews")]
[ApiController]
public class AdminReviewController(IAdminReviewService adminReviewService) : ControllerBase
{
    private readonly IAdminReviewService _adminReviewService = adminReviewService;

    // 1. جلب كل التقييمات للجدول
    [HttpGet("list")]
    public async Task<IActionResult> GetReviews([FromQuery] string? status, CancellationToken cancellationToken)
    {
        var result = await _adminReviewService.GetReviewsAsync(status, cancellationToken);
        return Ok(result);
    }

    // 2. جلب إحصائيات التقييمات (الـ Card اللي على الشمال)
    [HttpGet("analytics")]
    public async Task<IActionResult> GetReviewAnalytics(CancellationToken cancellationToken)
    {
        var result = await _adminReviewService.GetReviewAnalyticsAsync(cancellationToken);
        return Ok(result);
    }

    // 3. تعديل ظهور التقييم (Visible أو Flagged) عند الضغط على الأزرار
    [HttpPost("moderate")]
    public async Task<IActionResult> ModerateReview([FromBody] ModerateReviewRequest request, CancellationToken cancellationToken)
    {
        var result = await _adminReviewService.ModerateReviewAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}