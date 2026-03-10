using Khdamatk.Server.Contracts.Dashboard;
using Khdamatk.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class ReportDashboardController(IReportDashboardService _reportService) : ControllerBase
{
    // 1. ملخص الكروت (Total, Open, Resolved)
    [HttpGet("summary")]
    public async Task<IActionResult> GetReportSummary(CancellationToken ct)
        => (await _reportService.GetReportSummary(ct)).Respond();

    // 2. جدول البلاغات (مع الفلترة والـ Pagination)
    [HttpGet("list")]
    public async Task<IActionResult> GetReports(
        [FromQuery] string? search,
        [FromQuery] string? status,
        [FromQuery] string? type,
        CancellationToken ct,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 5
        ) // الـ ct في الآخر، كدة الأرور اختفى
    {
        var result = await _reportService.GetReportsList(search, status, type, page, pageSize, ct);
        return result.Respond();
    }

    // 3. صفحة التحقيق (الدردشة والمرفقات)
    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetDetails(string id, CancellationToken ct)
        => (await _reportService.GetReportDetails(id, ct)).Respond();

    [HttpPost("{id}/send-message")]
    public async Task<IActionResult> SendMessage(string id, [FromBody] string message, CancellationToken ct)
        => (await _reportService.SendReportMessage(id, message, ct)).Respond();

    // 4. الأكشنز السريعة (Verify, Block)
    [HttpPost("action")]
    public async Task<IActionResult> ExecuteAction([FromBody] ReportActionRequest req)
        => (await _reportService.ExecuteReportAction(req)).Respond();

    // 5. صفحة القرار النهائي والتعويضات
    [HttpGet("{id}/final-summary")]
    public async Task<IActionResult> GetFinalSummary(string id, CancellationToken ct)
        => (await _reportService.GetFinalDecisionSummary(id, ct)).Respond();

    [HttpPost("submit-decision")]
    public async Task<IActionResult> SubmitDecision([FromBody] SubmitDecisionRequest request)
        => (await _reportService.ConfirmFinalDecision(request)).Respond();
}