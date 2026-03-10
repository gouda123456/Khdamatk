using Khdamatk.Server.Contracts.Dashboard;
using Khdamatk.Server.Data.Entities.Identity; 
using Microsoft.EntityFrameworkCore; 

namespace Khdamatk.Server.Services;

public class ReportDashboardService(Database db) : IReportDashboardService
{
    private readonly Database _db = db;

    public async Task<resultBase> GetReportSummary(CancellationToken ct)
    {
        var total = await _db.Reports.CountAsync(ct);
        var open = await _db.Reports.CountAsync(r => r.Status == "Open", ct);
        var resolved = await _db.Reports.CountAsync(r => r.Status == "Resolved", ct);

        return Success(StatusCodes.Status200OK, new ReportStatsResponse(total, open, resolved));
    }

    public async Task<resultBase> GetReportsList(string? search, string? status, string? type, int page, int pageSize, CancellationToken ct)
    {
        var query = _db.Reports.AsQueryable();

        if (!string.IsNullOrEmpty(status) && status != "All Status")
            query = query.Where(r => r.Status == status);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(r => r.Id.Contains(search) || r.ClientName.Contains(search));

        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new ReportListItem(r.Id, r.JobId, r.ClientName, r.FreelancerName, r.Type, r.Status, r.CreatedAt))
            .ToListAsync(ct);

        return Success(StatusCodes.Status200OK, data);
    }

    public async Task<resultBase> GetReportDetails(string reportId, CancellationToken ct)
    {
        var report = await _db.Reports
            .Include(r => r.Attachments)
            .Include(r => r.Messages.OrderBy(m => m.Timestamp)) 
            .FirstOrDefaultAsync(r => r.Id == reportId, ct);

        if (report == null) return Failure(StatusCodes.Status404NotFound, UserErrors.RefreshTokenDoesNotExists);

        var detail = new ReportDetailResponse(
            report.Id,
            report.Reason,        // Missed Deadlines...
            report.Description,   // Detailed Description
            report.JobId,         // #JOB-5891
            report.ClientName,    // Sarah Johnson
            report.FreelancerName, // Michael Chen
            report.CreatedAt.ToString("MMM dd, yyyy"), // Jan 15, 2024
                                                     
report.Attachments.Select(a => new AttachmentDto(a.FileName, a.Url, a.Type)).ToList(),

// سطر 55: غير Content لـ Text وغير SentAt لـ Timestamp
report.Messages.Select(m => new ChatMessageDto(m.SenderName, m.Text, m.Timestamp.ToString("t"), m.IsAdmin)).ToList()
        );

        return Success(StatusCodes.Status200OK, detail);
    }

    public async Task<resultBase> ExecuteReportAction(ReportActionRequest request)
    {
        var report = await _db.Reports.FindAsync(request.ReportId);
        if (report == null) return Failure(StatusCodes.Status404NotFound, UserErrors.RefreshTokenDoesNotExists);

        if (request.ActionType == "Assign")
        {
            report.ReviewedBy = request.TargetUserId;
            report.Status = "Under Review";
        }

        await _db.SaveChangesAsync();
        return Success(StatusCodes.Status200OK, $"Action {request.ActionType} completed successfully");
    }

    public async Task<resultBase> GetFinalDecisionSummary(string reportId, CancellationToken ct)
    {
        var report = await _db.Reports.FindAsync(new object[] { reportId }, ct);

        if (report == null) return Failure(StatusCodes.Status404NotFound, UserErrors.RefreshTokenDoesNotExists);

        // بنملى الـ DTO بكل البيانات اللي في الصورة
        var decision = new FinalDecisionResponse(
            report.Id,                              // #RPT-2024-001547
            report.Type,                            // Account Security Breach
            report.Status == "Resolved" ? "Claim Approved" : "Pending Approval", // Decision Status
            report.ReviewedBy ?? "Senior Admin",    // Reviewed By
            report.CreatedAt,                       // Submitted Date
            DateTime.Now,                           // Review Date (تاريخ المراجعة الحالي)
            report.AdminComment ?? "No explanation provided.", // Administrative Decision Explanation
            report.CompensationAmount ?? 0,         // مبلغ التعويض (مثل $2,450.00)
            "USD"
        );

        return Success(StatusCodes.Status200OK, decision);
    }

    public async Task<resultBase> ConfirmFinalDecision(SubmitDecisionRequest request)
    {
        var report = await _db.Reports.FindAsync(request.ReportId);
        if (report == null) return Failure(StatusCodes.Status404NotFound, UserErrors.RefreshTokenDoesNotExists);

        // تحديث الحالة بناءً على الزرار اللي انضغط
        report.Status = request.IsApproved ? "Resolved" : "Rejected";
        report.AdminComment = request.AdminComment;

        await _db.SaveChangesAsync();
        return Success(StatusCodes.Status200OK, request.IsApproved ? "Decision Approved Successfully" : "Decision Rejected");
    }
    public async Task<resultBase> SendReportMessage(string reportId, string message, CancellationToken ct)
    {
        // 1. هات التقرير الأول مع الرسائل بتاعته
        var report = await _db.Reports
            .Include(r => r.Messages)
            .FirstOrDefaultAsync(r => r.Id == reportId, ct);

        if (report == null) return Failure(StatusCodes.Status404NotFound, UserErrors.RefreshTokenDoesNotExists);


        report.Messages.Add(new ReportMessage
        {
            Text = message,
            SenderName = "Admin",
            IsAdmin = true,
            Timestamp = DateTime.Now
        });

        // 3. سيف التغييرات
        await _db.SaveChangesAsync(ct);

        return Success(StatusCodes.Status200OK, "Message sent");
    }
}