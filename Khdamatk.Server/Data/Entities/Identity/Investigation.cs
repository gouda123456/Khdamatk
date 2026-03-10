using System.ComponentModel.DataAnnotations;

namespace Khdamatk.Server.Data.Entities.Identity;

public class ReportAttachment
{
    public ReportAttachment() { }

    [Key]
    public virtual string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public virtual string FileName { get; set; } = string.Empty;

    [Required]
    [Url(ErrorMessage = "Invalid file URL format")]
    public virtual string Url { get; set; } = string.Empty;

    public virtual string Type { get; set; } = string.Empty;
}

public class ReportMessage
{
    public ReportMessage() { }

    [Key]
    public virtual string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public virtual string SenderName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Message text is required")]
    [StringLength(1000, ErrorMessage = "Message is too long")]
    public virtual string Text { get; set; } = string.Empty;

    public virtual DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public virtual bool IsAdmin { get; set; }
}