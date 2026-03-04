using Khdamatk.Server.Contracts.Dashboard;
using Khdamatk.Server.Services.Interfaces;

namespace Khdamatk.Server.Services;

public class ReportDashboardService(Database db) : IReportDashboardService
{
    private readonly Database _db = db;

    public async Task<resultBase> GetReportSummary(CancellationToken ct) =>
        Success(StatusCodes.Status200OK, new ReportStatsResponse(247, 42, 205));

    public async Task<resultBase> GetReportsList(string? search, string? status, string? type, CancellationToken ct)
    {
        var reports = new List<ReportListItem>
        {
            new ("#RPT-001", "#JOB-2024-001", "Sarah Johnson", "Mike Chen", "Payment Issue", "Open", new DateTime(2024, 01, 15)),
            new ("#RPT-002", "#JOB-2024-002", "David Wilson", "Emma Davis", "Quality Concern", "Under Review", new DateTime(2024, 01, 14)),
            new ("#RPT-003", "#JOB-2024-003", "Lisa Anderson", "James Miller", "Communication", "Resolved", new DateTime(2024, 01, 13))
        };

        var filtered = reports.Where(r =>
            (string.IsNullOrEmpty(search) || r.ReportId.Contains(search) || r.ClientName.Contains(search, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(status) || r.Status == status)
        ).ToList();

        return Success(StatusCodes.Status200OK, filtered);
    }

    public async Task<resultBase> GetReportDetails(string reportId, CancellationToken ct)
    {
        var detail = new ReportDetailResponse(
            reportId, "Missed Deadlines & Poor Communication", "The freelancer consistently missed deadlines...",
            "#JOB-5891", "Sarah Johnson", "Michael Chen", "Jan 15, 2024",
            new List<AttachmentDto> { new("evidence.pdf", "/files/1.pdf", "pdf") },
            new List<ChatMessageDto> { new("Sarah", "I need help", "10:23 AM", false) }
        );
        return Success(StatusCodes.Status200OK, detail);
    }

    public async Task<resultBase> ExecuteReportAction(ReportActionRequest request) =>
        Success(StatusCodes.Status200OK, $"Action {request.ActionType} executed");

    public async Task<resultBase> GetFinalDecisionSummary(string reportId, CancellationToken ct)
    {
        var decision = new FinalDecisionResponse(
            "#RPT-2024-001547", "Account Security Breach", "Claim Approved", "Senior Admin",
            DateTime.Now.AddDays(-5), DateTime.Now, "Approved full compensation.", 2450.00m, "USD"
        );
        return Success(StatusCodes.Status200OK, decision);
    }

    public async Task<resultBase> ConfirmFinalDecision(SubmitDecisionRequest request) =>
        Success(StatusCodes.Status200OK, "Decision confirmed.");
}