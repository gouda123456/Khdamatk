namespace Khdamatk.Server.Data.Entities.Catalog;


public class JobOffer
{
    public int Id { get; set; } 

    public string Description { get; set; } = string.Empty;
    
    public int DeliveryTimeInDays { get; set; }
    public string? SimilarWorkExamplesURL { get; set; }
    public JobOfferStatus Status { get; set; } = JobOfferStatus.Pending;
    public ExperienceLevel ExperienceLevel { get; set; } = ExperienceLevel.Entry;

    
    public decimal Amount { get; set; }

    // لتحديد هل هذا العرض هو "العرض الفائز" الذي تحول لطلب (Order)
    
    public bool IsAccepted { get; set; } = false;


    // الربط مع الوظيفة الأساسية
    public int JobPostId { get; set; }
    public virtual JobPost JobPost { get; set; } = null!;
    public TimeCommit? TimeCommitment { get; set; } = TimeCommit.PartTime;

    // الربط مع ملف مقدم الخدمة (الفريلانسر)
    public string ProviderProfileId { get; set; }
    public virtual ServiceProviderProfile ProviderProfile { get; set; }

    // --- الربط مع المحادثة حسب رؤيتك ---
    // العرض هو الذي يشير إلى المحادثة لبدء التفاوض
    public int? ConversationId { get; set; }
    public virtual Conversation? Conversation { get; set; }

    [ForeignKey(nameof(Order))]
    public int? OrderId { get; set; }
    public virtual JobOrder? Order { get; set; }

    public virtual ICollection<Media> Attachments { get; set; } = new List<Media>();



    public static List<JobOffer> Data(int minId)
    {
        var list = new List<JobOffer>();

        for (int i = minId; i < minId + 5; i++)
        {
            list.Add(new JobOffer
            {
                Description = $"Job Offer {i}",
                DeliveryTimeInDays = i,
                SimilarWorkExamplesURL = $"https://example.com/work/{i}",
                Status = JobOfferStatus.Pending,
                ExperienceLevel = ExperienceLevel.Entry,
                Amount = 100 + i,
                IsAccepted = false,
                JobPostId = i,
                ProviderProfileId = $"Provider{i}",
                ConversationId = i,
                OrderId = i
            });
        }

        return list;
    }
}

public enum JobOfferStatus
{
    Pending = 1,
    Accepted = 2,
    Rejected = 3,
    Withdrawn = 4
}
