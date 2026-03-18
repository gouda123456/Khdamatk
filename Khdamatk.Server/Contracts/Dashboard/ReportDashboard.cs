namespace Khdamatk.Server.Contracts.Dashboard;



/// Represents a report row in the Report Management list. 
public record ReportListItem(
    string ReportId,
    string JobId,
    string ClientName,
    string FreelancerName,
    string ReportType,
    string Status,
    DateTime CreatedDate
);

/// Detailed investigation data including chat history and evidence. 
public record ReportDetailResponse(
    string ReportId,
    string ReportReason,
    string DetailedDescription,
    string JobId,
    string ClientName,
    string FreelancerName,
    string DateReported,
    List<AttachmentDto> Attachments,
    List<ChatMessageDto> Messages
);

public record ChatMessageDto(string SenderName, string Message, string Time, bool IsAdmin);
public record AttachmentDto(string FileName, string FileUrl, string FileType);




/// Final administrative resolution and financial compensation details. 
public record FinalDecisionResponse(
    string ReportId,
    string ReportType,
    string DecisionStatus,
    string ReviewedBy,
    DateTime SubmittedDate,
    DateTime ReviewDate,
    string AdministrativeExplanation,
    decimal CompensationAmount,
    string Currency = "USD"
);
public record SendMessageRequest(string Message); 
///  Request to execute an immediate action on a report. 
public record ReportActionRequest(string ReportId, string ActionType, string TargetUserId);

/// Final submission to approve or reject a claim. 
public record SubmitDecisionRequest(string ReportId, bool IsApproved, string AdminComment);