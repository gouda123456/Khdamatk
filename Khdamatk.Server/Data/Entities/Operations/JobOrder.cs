namespace Khdamatk.Server.Data.Entities.Operations;

public class JobOrder : BaseEntity
{
    // الربط مع الوظيفة والعرض
    public int JobPostId { get; set; }
    public int AcceptedOfferId { get; set; }

    // أطراف التعاقد (للسرعة في الـ Queries)
    public string CustomerId { get; set; } = null!;
    public string ProviderProfileId { get; set; } = null!;

    // البيانات المالية والزمنية
    public decimal FinalPrice { get; set; }
    public DateTime ExpectedDeliveryDate { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Active;

    // العلاقات (Navigation Properties)
    public virtual JobPost JobPost { get; set; } = null!;
    public virtual JobOffer AcceptedOffer { get; set; } = null!;
    public virtual User Customer { get; set; } = null!;
    public virtual ServiceProviderProfile ProviderProfile { get; set; } = null!;

    // الملحقات والتقييم والمالية
    public virtual ICollection<JobDeliverable> Deliverables { get; set; } = [];
    public virtual Review? Review { get; set; } // تقييم هذا العقد
    public virtual PaymentTransaction? PaymentTransaction { get; set; } // المعاملة المالية المرتبطة
}