using Khdamatk.Server.Contracts.Dashboard;

namespace Khdamatk.Server.Services.Interfaces;

public interface IReportDashboardService : IService
{
    ///  Gets summary counts for the report screen (Total, Open, Resolved). 
    Task<resultBase> GetReportSummary(CancellationToken ct);

    /// Lists all reports with filtering by status and type. 

    Task<resultBase> GetReportsList(string? search, string? status, string? type, int page, int pageSize, CancellationToken ct);

    /// Fetches chat history and full details for a specific report. 
    Task<resultBase> GetReportDetails(string reportId, CancellationToken ct);

    ///  Executes immediate moderation actions (Verify Report, Block User). 
    Task<resultBase> ExecuteReportAction(ReportActionRequest request);

    ///  Retrieves the final compensation and resolution summary. 
    Task<resultBase> GetFinalDecisionSummary(string reportId, CancellationToken ct);

    /// Confirms the final administrative decision (Approve/Reject Claim).
    Task<resultBase> ConfirmFinalDecision(SubmitDecisionRequest request);
    Task<resultBase> SendReportMessage(string reportId, string message, CancellationToken ct);
}