namespace Khdamatk.Server.Contracts.Reports;

public record ReviewReportRequest(
    [Required] string Status, // e.g., "Investigating", "Resolved", "Rejected"
    string? AdminComment,
    [Range(0, 100000)] decimal? CompensationAmount
);

// لإضافة رسالة جديدة داخل الـ Dispute/Report (محادثة الدعم)
public record AddReportMessageRequest(
    [Required] string MessageText
);
