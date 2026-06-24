namespace Khdamatk.Server.Contracts.Reports;

public record ReportResponse(
    string Id,
    string JobId,
    string ClientName,
    string FreelancerName,
    string Type,
    string Reason,
    string Description,
    string Status,
    DateTime CreatedAt,
    string? AdminComment,
    decimal? CompensationAmount,
    string? ReviewedBy,
    IReadOnlyCollection<ReportAttachmentResponse> Attachments,
    IReadOnlyCollection<ReportMessageResponse> Messages
);