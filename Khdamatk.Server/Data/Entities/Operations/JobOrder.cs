namespace Khdamatk.Server.Data.Entities.Operations;

public class JobOrder : OrderBase
{
    // الربط مع الوظيفة والعرض
    public int JobPostId { get; set; }
    public int AcceptedOfferId { get; set; }

    // أطراف التعاقد (للسرعة في الـ Queries)
    public string CustomerId { get; set; } = null!;
    public string ProviderProfileId { get; set; } = null!;


    public DateTime ExpectedDeliveryDate { get; set; }
    


    


    // العلاقات (Navigation Properties)
    public virtual JobPost JobPost { get; set; } = null!;
    public virtual JobOffer AcceptedOffer { get; set; } = null!;
    public virtual User Customer { get; set; } = null!;
    public virtual ServiceProviderProfile ProviderProfile { get; set; } = null!;
    public virtual Conversation Conversation { get; set; } = null!;

    // الملحقات والتقييم والمالية
    public virtual ICollection<JobDeliverable> Deliverables { get; set; } = [];
    
    
    public virtual List<JobOffer> Offers { get; set; } = [];


    public JobOrder BuildOrder(JobPost job ,  JobOffer offer)
    {
        var order = new JobOrder()
        {

            AcceptedOfferId = offer.Id,
            JobPostId = job.Id,
            CustomerId = job.CustomerId,
            ProviderProfileId = offer.ProviderProfileId,
            ExpectedDeliveryDate = offer.Deadline,

            Status = OrderStatus.Pending,
            Amount = offer.ProposedPrice,

            Conversation = new Conversation
            {
                Category = ConversationCategory.Standard,
                ClientId = job.CustomerId,
                ContextType = ConversationContextType.JobOffer,
                ProviderId = offer.ProviderProfileId,
                Title = job.Title,

            }
        };

        return order;
    }
}