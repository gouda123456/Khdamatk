namespace Khdamatk.Server.Contracts.Reports;

public record CreateReportRequest(
    [Required] string JobId,
    [Required(ErrorMessage = "Client name is required")] string ClientName,
    [Required(ErrorMessage = "Freelancer name is required")] string FreelancerName,
    [Required] string Type,
    [Required(ErrorMessage = "Report reason is required")] string Reason,
    [Required(ErrorMessage = "Description is required")]
    [StringLength(4000, ErrorMessage = "Description is too long")] string Description,
    List<CreateAttachmentRequest> Attachments
);
