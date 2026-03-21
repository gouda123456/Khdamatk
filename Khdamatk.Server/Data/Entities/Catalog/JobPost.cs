namespace Khdamatk.Server.Data.Entities.Catalog;

public class JobPost
{
    public int Id { get; set; }

    [ForeignKey(nameof(Customer))]
    public string CustomerId { get; set; } = string.Empty;

    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(Order))]
    public int OrderId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BudgetMin { get; set; }
    public decimal BudgetMax { get; set; }
    

    public JobPostStatus Status { get; set; } = JobPostStatus.Open;
    
    public ExperienceLevel ExperienceLevel { get; set; } 
    public string ProjectLength { get; set; } = string.Empty;
    public TimeCommit TimeCommitment { get; set; } = TimeCommit.PartTime; 

    public DateTime Deadline { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



    // Navigation Properties
    public virtual User Customer { get; set; } = null!;
    public virtual Category Category { get; set; } = null!;

    // العلاقة مع العروض (One-to-Many)
    public virtual ICollection<MileStone> MileStones { get; set; } = [];
    public virtual JobOrder? Order {  get; set; }
    public virtual ICollection<JobOffer>? Offers { get; set; } = [];
    public virtual ICollection<Media>? Media { get; set; } =[];
    public virtual ICollection<DeliveredJobFile> DeliveredFiles { get; set; } = [];
    public virtual ICollection<JobSkillRequirement> SkillRequirements { get; set; } = [];
}
 
public enum JobPostStatus
{
    Open = 1,
    InProgress = 2,
    Completed = 3,
    Cancelled = 4,
    Closed = 5
}
public enum ExperienceLevel
{
    Entry = 1,
    Intermediate = 2,
    Expert = 3
}