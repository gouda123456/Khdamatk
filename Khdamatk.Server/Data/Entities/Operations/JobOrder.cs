namespace Khdamatk.Server.Data.Entities.Operations;

public class JobOrder : OrderBase
{
    // الربط مع الوظيفة والعرض
    [ForeignKey(nameof(Job))]
    public int JobPostId { get; set; }
    [ForeignKey(nameof(AcceptedOffer))]
    public int AcceptedOfferId { get; set; }

    


    public DateTime ExpectedDeliveryDate { get; set; }
    
    // العلاقات (Navigation Properties)
    public virtual JobPost Job { get; set; } = null!;
    public virtual JobOffer AcceptedOffer { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Customer))]
    public string CustomerId { get; set; } = null!; // العميل (من قام بالطلب)
    public virtual User Customer { get; set; } = null!;

    

    // ✅ هذا الرابط صحيح ويضمن السرعة في الاستعلام عن صاحب العمل
    [Required]
    [ForeignKey(nameof(ServiceProviderProfile))]
    public string ServiceProviderId { get; set; } = null!;

    public virtual ServiceProviderProfile ServiceProviderProfile { get; set; } = null!;

    [ForeignKey(nameof(PaymentTransaction))]
    public int PaymentTransactionId { get; set; }
    public virtual PaymentTransaction PaymentTransaction { get; set; } = new();


    [ForeignKey(nameof(Review))]
    public int? ReviewId { get; set; }
    public virtual Review? Review { get; set; }

    [ForeignKey(nameof(Conversation))]
    public int ConversationId { get; set; }
    public virtual Conversation Conversation { get; set; } = new Conversation();

    [ForeignKey(nameof(Dispute))]
    public int? DisputeId { get; set; }
    public virtual Dispute? Dispute { get; set; }

    // الملحقات والتقييم والمالية


    public virtual ICollection<Media>? MediaAttachments { get; set; } = []; // المرفقات (صور، ملفات، إلخ) 

    public static JobOrder BuildOrder(JobPost job, JobOffer offer)
    {
        var order = new JobOrder()
        {

            AcceptedOfferId = offer.Id,//
            JobPostId = job.Id,//
            CustomerId = job.CustomerId,//
            ServiceProviderId = offer.ProviderProfileId,//
            ExpectedDeliveryDate = DateTime.UtcNow.AddDays( offer.DeliveryTimeInDays),//

            Status = OrderStatus.Pending,//
            Amount = offer.Amount,//

            Conversation = new Conversation//
            {
                Category = ConversationCategory.Standard,
                CustomerId = job.CustomerId,
                ContextType = ConversationContextType.JobOffer,
                ProviderId = offer.ProviderProfileId,
                Title = job.Title,

            }
        };

        
        return order;
    }
    
}