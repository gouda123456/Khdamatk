namespace Khdamatk.Server.Data.Entities.Interaction;


public class Conversation : BaseEntity
{

    public string Title { get; set; } = string.Empty;

    public ICollection<Message> Messages { get; set; } = [];

    // Foreign Key to ServiceOrder
    public int ServiceOrderId { get; set; }
    public virtual ServiceOrder ServiceOrder { get; set; } = null!;

    // Foreign Key to User (Sender)
    [ForeignKey(nameof(Client))]
    public string ClientId { get; set; } = null!;
    public virtual User Client { get; set; } = null!;

    //Conversation Category
    public ConversationCategory Category { get; set; } = ConversationCategory.Standard;

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