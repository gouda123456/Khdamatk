using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Khdamatk.Server.Data.Entities.Identity;

public class VerificationData
{
    [Key]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = null!;

    public string NationalNumber { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    // الحقول الجديدة لروابط الصور اللي كانت ناقصة ومسببة الأيرور
    public string IdFrontUrl { get; set; } = string.Empty;
    public string IdBackUrl { get; set; } = string.Empty;
    public string SelfieWithIdUrl { get; set; } = string.Empty;

    // حقل لتخزين ملاحظات الأدمن في حالة الرفض
    public string? RejectNotes { get; set; }

    public VerificationStatus Status { get; set; } = VerificationStatus.Pending;

    public virtual User User { get; set; } = null!;
}

public enum VerificationStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2
}