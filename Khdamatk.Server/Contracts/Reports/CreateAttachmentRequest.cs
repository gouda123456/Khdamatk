namespace Khdamatk.Server.Contracts.Reports;

// لرفع مرفق جديد مع البلاغ
public record CreateAttachmentRequest(
    [Required] string FileUrl,
    string? FileName
);

// لعرض المرفقات
public record ReportAttachmentResponse(
    string Id,
    string FileUrl,
    string? FileName,
    DateTime UploadedAt
);

// لعرض الرسائل المتبادلة داخل البلاغ
public record ReportMessageResponse(
    string Id,
    string SenderId,
    string SenderName,
    string MessageText,
    DateTime SentAt
);