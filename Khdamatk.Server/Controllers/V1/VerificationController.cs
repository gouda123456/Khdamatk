using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Khdamatk.Server.Contracts.Verification;
using Khdamatk.Server.Services.Interfaces;

namespace Khdamatk.Server.Controllers.V1;



// [Authorize] <-- شيلها مؤقتاً
[Route("api/[controller]")]
[ApiController]
public class VerificationController(IVerificationService verificationService) : ControllerBase
{
    private readonly IVerificationService _verificationService = verificationService;

    [HttpPost("submit-request")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> SubmitVerification([FromForm] SubmitVerificationRequest request, CancellationToken cancellationToken)
    {
        // باصي الـ ID بتاع أي مستخدم مسجل عندك في جدول الـ Users يدوي هنا للتجربة
        var userId = "019f053f-8c15-7c98-b6ad-092c9b82cb67";

        var result = await _verificationService.SubmitVerificationAsync(request, userId, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetStatus(CancellationToken cancellationToken)
    {
        // نفس الـ ID هنا برضه
        var userId = "019f053f-8c15-7c98-b6ad-092c9b82cb67";

        var result = await _verificationService.GetVerificationStatusAsync(userId, cancellationToken);
        return Ok(result);
    }
}