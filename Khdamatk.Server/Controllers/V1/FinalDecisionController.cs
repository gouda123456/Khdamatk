using Microsoft.AspNetCore.Mvc;
using Khdamatk.Server.Services.Interfaces;
using Khdamatk.Server.Contracts.Admin.Disputes;

namespace Khdamatk.Server.Controllers.Admin;

[Route("api/admin/final-decision")]
[ApiController]
public class FinalDecisionController(IFinalDecisionService finalDecisionService) : ControllerBase
{
    private readonly IFinalDecisionService _finalDecisionService = finalDecisionService;

    // 1. endpoint لجلب بيانات الصفحة بالكامل
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDecisionDetails([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _finalDecisionService.GetDecisionDetailsAsync(id, cancellationToken);
        return Ok(result);
    }

    // 2. endpoint لحفظ القرار النهائي ورفع الصور (بنستخدم FromForm عشان في List من الملفات مرفوعة)
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitDecision([FromForm] SubmitDecisionRequest request, CancellationToken cancellationToken)
    {
        var result = await _finalDecisionService.SubmitDecisionAsync(request, cancellationToken);
        return Ok(result);
    }
}