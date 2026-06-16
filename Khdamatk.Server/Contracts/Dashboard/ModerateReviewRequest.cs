using System.ComponentModel.DataAnnotations;

namespace Khdamatk.Server.Contracts.Admin.Review;

public class ModerateReviewRequest
{
    [Required]
    public int ReviewId { get; set; }

    [Required]
    // بنستقبل الـ Status الجديدة (إما "Visible" أو "Flagged")
    public string NewStatus { get; set; } = string.Empty;
}