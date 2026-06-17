using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Khdamatk.Server.Contracts.Admin.Verification;
using Khdamatk.Server.Services.Interfaces;

namespace Khdamatk.Server.Controllers.Admin;

[Authorize(Roles = "Admin")] 
[Route("api/admin/verification")]
[ApiController]
public class AdminVerificationController(IAdminVerificationService adminVerificationService) : ControllerBase
{
    private readonly IAdminVerificationService _adminVerificationService = adminVerificationService;

    // 1. الـ API الخاص بجلب لستة طلبات التوثيق كلها للجدول
    [HttpGet("list")]
    public async Task<IActionResult> GetVerifications([FromQuery] string? status, CancellationToken cancellationToken)
    {
        var result = await _adminVerificationService.GetPendingVerificationsAsync(status, cancellationToken);
        return Ok(result);
    }

    // 2. الـ API الخاص باتخاذ إجراء (قبول أو رفض) عند الضغط على الأزرار
    [HttpPost("review")]
    public async Task<IActionResult> ReviewVerification([FromBody] ReviewVerificationRequest request, CancellationToken cancellationToken)
    {
        var result = await _adminVerificationService.ReviewVerificationAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}