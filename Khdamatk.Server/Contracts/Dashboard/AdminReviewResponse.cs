namespace Khdamatk.Server.Contracts.Admin.Review;

public class AdminReviewResponse
{
    public int ReviewId { get; set; }
    public string ReviewerName { get; set; } = string.Empty; // اسم الشخص اللي قَيّم
    public string ReviewerImageUrl { get; set; } = string.Empty; // صورة العميل
    public int Rating { get; set; } // عدد النجوم (من 1 لـ 5)
    public string ReviewText { get; set; } = string.Empty; // نص التقييم
    public string Status { get; set; } = string.Empty; // Visible أو Flagged
    public DateTime CreatedAt { get; set; } // تاريخ التقييم
}