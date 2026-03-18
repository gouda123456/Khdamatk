namespace Khdamatk.Server.Data.Entities.Identity;

public class ServiceProviderProfile
{
    [Key]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = string.Empty;
    public virtual User User { get; set; } = null!;

    public bool IsActive { get; set; } = true;
    public bool IsAvailable { get; set; } = true;

    public DateTime DateOfJoin { get; set; } = DateTime.UtcNow;
    public DateTime? LastActiveDate { get; set; }
    public DateTime? LastUpdate { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string JobTitle { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, MinimumLength = 10)]
    public string Bio { get; set; } = string.Empty;

    // --- Social Media Links (التعديل الجديد هنا) ---
    // جعلناها nullable (?) لأنها اختيارية في الفورم
    [Url]
    public string? FacebookUrl { get; set; }

    [Url]
    public string? LinkedInUrl { get; set; }

    [Url]
    public string? GithubUrl { get; set; }

    [Url]
    public string? TwitterUrl { get; set; }

    public int TotalReviews { get; set; } = 0;
    public double AverageRating { get; set; } = 0;
    public int CompletedJobs { get; set; } = 0;

    public int ExperienceYears { get; set; } = 0;

    [Range(1, 168)]
    public double WorkingHoursPerWeek { get; set; } = 1;

    [Range(1, 10000)]
    public double HourlyRate { get; set; } = 1;

    public int AverageResponseTime { get; set; } = 0; // بالساعة

    // --- العلاقات ---
    public virtual ICollection<ProviderSkill> Skills { get; set; } = [];
    public virtual ICollection<Service> Services { get; set; } = [];
    public virtual ICollection<Certificate> Certificates { get; set; } = [];
    public virtual ICollection<PortfolioItem> PortfolioItems { get; set; } = [];
    public virtual ICollection<Review> Reviews { get; set; } = [];
    public virtual ICollection<JobOffer> JobOffers { get; set; } = [];
}