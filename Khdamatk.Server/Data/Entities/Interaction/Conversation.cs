namespace Khdamatk.Server.Data.Entities.Interaction;


public class Conversation : BaseEntity
{
    [Required]
    public int RelatedEntityId { get; set; }



    public string Title { get; set; } = string.Empty;

    public virtual ICollection<Message> Messages { get; set; } = [];

    // Foreign Key to ServiceOrder
    [ForeignKey(nameof(ServiceOrder))]
    public int? ServiceOrderId { get; set; }
    public virtual ServiceOrder? ServiceOrder { get; set; } = null!;

    [ForeignKey(nameof(JobOrder))]
    public int? JobOrderId { get; set; }
    public virtual JobOrder? JobOrder { get; set; } = null!;

    // Foreign Key to User (Sender)
    [ForeignKey(nameof(Customer))]
    public string CustomerId { get; set; } = null!;
    public virtual User Customer { get; set; } = null!;

    //Conversation Category
    public ConversationCategory Category { get; set; } = ConversationCategory.Standard;

    [Required]
    public ConversationContextType ContextType { get; set; } = ConversationContextType.General;

    // Foreign Key to User (Receiver)
    [ForeignKey(nameof(Provider))]
    public string ProviderId { get; set; } = null!;
    public virtual User Provider { get; set; } = null!;

}

public enum ConversationCategory
{
    Standard = 1,      // محادثة عادية بين العميل ومقدم الخدمة
    DisputeRaiser = 2, // محادثة نزاع (مسؤول + رافع)
    DisputeTarget = 3  // محادثة نزاع (مسؤول + مدعى عليه)
}

public enum ConversationContextType
{
    General = 0,      // محادثة عامة
    ServiceOrder = 1, // مرتبطة بطلب خدمة
    JobOffer = 2,     // مرتبطة بعرض عمل
    Dispute = 3       // مرتبطة بنزاع
}