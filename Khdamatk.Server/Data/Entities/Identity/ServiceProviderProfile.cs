namespace Khdamatk.Server.Data.Entities.Identity;

//TODO: check if there important properties are needed
public class ServiceProviderProfile 
{
    [Key]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = string.Empty;

    public User User { get; set; } = null!;

    public bool IsActive { get; set; } = true;


    public DateTime DateOfJoin { get; set; } = DateTime.UtcNow;
    public DateTime? LastActiveDate { get; set; }
    public DateTime? LastUpdate { get; set; }


    [Required] // يُفضل أن يكون إلزامي
    [StringLength(50, MinimumLength = 2)] // قيد مقترح لاسم الوظيفة
    public string JobTitle { get; set; } = string.Empty;

    // ...

    [Required] // يُفضل أن يكون إلزامي
    [StringLength(500, MinimumLength = 10)] // استخدام الصيغة الصحيحة
    public string Bio { get; set; } = string.Empty;

    public int TotalReviews { get; set; } = 0;

    //TODO: Make it Computed Column in the Database
    public double AverageRating { get; set; } = 0;


    public int ExperienceYears { get; set; } = 0;

    [Range(1,40)]
    public double WorkingHoursPerWeek { get; set; } = 1;

    [Range(1, 1000)]
    public double HourlyRate { get; set; } = 1;





    public virtual List<ProviderSkill>? Skills { get; set; } = [];
    public virtual List<Service>? Services { get; set; } = [];
          
          
    public virtual List<Certificate>? Certificates { get; set; } = [];
          
    public virtual List<PortfolioItem>? PortfolioItems { get; set; } = [];

    

}
