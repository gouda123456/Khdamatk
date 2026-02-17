namespace Khdamatk.Server.Data.Entities.Operations;

public class JobOrder : BaseEntity
{
    public int JobPostId { get; set; }
    public int AcceptedOfferId { get; set; } // العرض الذي تم قبوله

    // البيانات المالية النهائية وقت التعاقد
    public decimal FinalPrice { get; set; }
    public DateTime ExpectedDeliveryDate { get; set; }

    public OrderStatus Status { get; set; } // Active, UnderReview, Completed

    public virtual JobPost JobPost { get; set; }
    public virtual JobOffer AcceptedOffer { get; set; }
}