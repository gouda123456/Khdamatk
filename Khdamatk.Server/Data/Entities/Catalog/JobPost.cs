namespace Khdamatk.Server.Data.Entities.Catalog;

public class JobPost
{
    public int Id { get; set; } 

    // المفتاح الأجنبي للعميل (User) - افترضت أن الـ Id هنا من نوع string بناءً على Identity الافتراضي
    public string CustomerId { get; set; }

    // المفتاح الأجنبي للتصنيف (Category)
    public int CategoryId { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public decimal BudgetMin { get; set; }
    public decimal BudgetMax { get; set; }

    public JobPostStatus Status { get; set; } = JobPostStatus.Open;
    public DateTime Deadline { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual User Customer { get; set; }
    public virtual Category Category { get; set; }

    // العلاقة مع العروض (One-to-Many)
    public virtual ICollection<JobOffer> Offers { get; set; } = new HashSet<JobOffer>();
}

public enum JobPostStatus
{
    Open = 1,
    InProgress = 2,
    Completed = 3,
    Cancelled = 4
}