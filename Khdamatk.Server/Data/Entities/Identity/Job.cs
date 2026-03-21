using System.ComponentModel.DataAnnotations;

namespace Khdamatk.Server.Data.Entities.Identity;

public class Job
{
    public Job() { }

    [Key]
    public virtual string Id { get; set; } = Guid.NewGuid().ToString();

    [Required(ErrorMessage = "Job title is required")]
    [StringLength(200, MinimumLength = 10, ErrorMessage = "Title must be between 10 and 200 characters")]
    public virtual string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Job description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public virtual string Description { get; set; } = string.Empty;

    [Range(10, 100000, ErrorMessage = "Budget must be between 10 and 100,000")]
    public virtual decimal Budget { get; set; }

    [Required]
    public virtual string Status { get; set; } = "Open";

    public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public virtual string UserId { get; set; } = string.Empty;

    public virtual User? User { get; set; }
}