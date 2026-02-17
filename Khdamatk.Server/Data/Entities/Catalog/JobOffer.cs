namespace Khdamatk.Server.Data.Entities.Catalog;

public class JobOffer
{
    public int Id { get; set; } 

    public string CoverLetter { get; set; } = string.Empty;
    public decimal ProposedPrice { get; set; }
    public int DeliveryTimeInDays { get; set; }
    public JobOfferStatus Status { get; set; } = JobOfferStatus.Pending;

    // المبلغ الذي سيستلمه الفريلانسر فعلياً بعد خصم عمولة المنصة
    public decimal NetAmount { get; set; }

    // لتحديد هل هذا العرض هو "العرض الفائز" الذي تحول لطلب (Order)
    public bool IsAccepted { get; set; } = false;

    // الربط مع الوظيفة الأساسية
    public int JobPostId { get; set; }
    public virtual JobPost JobPost { get; set; } = null!;

    // الربط مع ملف مقدم الخدمة (الفريلانسر)
    public string ProviderProfileId { get; set; }
    public virtual ServiceProviderProfile ProviderProfile { get; set; } = null!;

    // --- الربط مع المحادثة حسب رؤيتك ---
    // العرض هو الذي يشير إلى المحادثة لبدء التفاوض
    public int? ConversationId { get; set; }
    public virtual Conversation? Conversation { get; set; }
}

public enum JobOfferStatus
{
    Pending = 1,
    Accepted = 2,
    Rejected = 3,
    Withdrawn = 4
}
