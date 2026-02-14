namespace Khdamatk.Server.Data.Entities.Identity;

public class ServiceProviderProfile 
{
    [Key]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = string.Empty;
    public virtual User User { get; set; } = null!;

    public bool IsActive { get; set; } = true;
    public bool IsAvailable { get; set; } = true; // متاح لاستقبال عروض جديدة

    public DateTime DateOfJoin { get; set; } = DateTime.UtcNow;
    public DateTime? LastActiveDate { get; set; }
    public DateTime? LastUpdate { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string JobTitle { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, MinimumLength = 10)] // رفعت الطول قليلاً للـ Bio الاحترافي
    public string Bio { get; set; } = string.Empty;

    // --- الإحصائيات ---
    public int TotalReviews { get; set; } = 0;
    public double AverageRating { get; set; } = 0; // Computed Column لاحقاً
    public int CompletedJobs { get; set; } = 0; // عدد الوظائف التي أنهاها بنجاح

    public int ExperienceYears { get; set; } = 0;

    [Range(1, 168)] // الأسبوع فيه 168 ساعة كحد أقصى
    public double WorkingHoursPerWeek { get; set; } = 1;

    [Range(1, 10000)]
    public double HourlyRate { get; set; } = 1;

    // --- العلاقات (Navigation Properties) ---
    
    public virtual ICollection<ProviderSkill> Skills { get; set; } = [];
    public virtual ICollection<Service> Services { get; set; } = [];
    public virtual ICollection<Certificate> Certificates { get; set; } = [];
    public virtual ICollection<PortfolioItem> PortfolioItems { get; set; } = [];

    public virtual ICollection<Review> Reviews { get; set; } = [];
    // الربط مع نظام العروض الجديد
    public virtual ICollection<JobOffer> JobOffers { get; set; } = [];
}