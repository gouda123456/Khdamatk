using System.ComponentModel.DataAnnotations;

namespace Khdamatk.Server.Data.Entities.Identity;

public class Report
{
    public Report() { }

    [Key]
    public virtual string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public virtual string JobId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Client name is required")]
    public virtual string ClientName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Freelancer name is required")]
    public virtual string FreelancerName { get; set; } = string.Empty;

    [Required]
    public virtual string Type { get; set; } = string.Empty;

    [Required(ErrorMessage = "Report reason is required")]
    public virtual string Reason { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(4000, ErrorMessage = "Description is too long")]
    public virtual string Description { get; set; } = string.Empty;

    [Required]
    public virtual string Status { get; set; } = "Pending";

    public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual string? AdminComment { get; set; }

    [Range(0, 100000)]
    public virtual decimal? CompensationAmount { get; set; }

    public virtual string? ReviewedBy { get; set; }

    public virtual ICollection<ReportAttachment> Attachments { get; set; } = new List<ReportAttachment>();
    public virtual ICollection<ReportMessage> Messages { get; set; } = new List<ReportMessage>();
}