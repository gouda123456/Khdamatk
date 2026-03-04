using Khdamatk.Server.Contracts.Dashboard;
using Khdamatk.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class ReportDashboardController(IReportDashboardService _reportService) : ControllerBase
{
    ///  GET summary cards: Total, Open, and Resolved reports. 
    [HttpGet("summary")]
    public async Task<IActionResult> GetReportSummary(CancellationToken ct)
        => (await _reportService.GetReportSummary(ct)).Respond();

    ///  GET All Reports with filtering. 
    [HttpGet("list")]
    public async Task<IActionResult> GetReports(
        [FromQuery] string? search,
        [FromQuery] string? status,
        [FromQuery] string? type,
        CancellationToken ct)
    {
        var result = await _reportService.GetReportsList(search, status, type, ct);
        return result.Respond();
    }

    ///  GET Detailed Report View: Chat, Summary, and Attachments. 
    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetDetails(string id, CancellationToken ct)
        => (await _reportService.GetReportDetails(id, ct)).Respond();

    /// Execute immediate actions like "Verify Report" or "Block User". 
    [HttpPost("action")]
    public async Task<IActionResult> TakeAction([FromBody] ReportActionRequest req)
        => (await _reportService.ExecuteReportAction(req)).Respond();

    ///  GET Final Decision Summary including compensation amount ($2,450.00). 
    [HttpGet("{id}/final-decision")]
    public async Task<IActionResult> GetFinalDecision(string id, CancellationToken ct)
        => (await _reportService.GetFinalDecisionSummary(id, ct)).Respond();

    ///  POST Confirm or Reject the financial decision. 
    [HttpPost("confirm-decision")]
    public async Task<IActionResult> ConfirmDecision([FromBody] SubmitDecisionRequest req)
        => (await _reportService.ConfirmFinalDecision(req)).Respond();
}