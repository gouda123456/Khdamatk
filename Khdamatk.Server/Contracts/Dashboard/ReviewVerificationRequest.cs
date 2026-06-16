using System.ComponentModel.DataAnnotations;

namespace Khdamatk.Server.Contracts.Admin.Verification;

public class ReviewVerificationRequest
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public bool IsApproved { get; set; } // True = Approve, False = Reject

    public string? Notes { get; set; } // ملاحظات الرفض إن وجدت
}
