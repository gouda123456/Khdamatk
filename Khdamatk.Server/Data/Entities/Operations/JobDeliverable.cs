namespace Khdamatk.Server.Data.Entities.Operations;

public class JobDeliverable : BaseEntity
{
    public int JobOrderId { get; set; }
    public string Description { get; set; } = null!;
    public virtual ICollection<Media> Attachments { get; set; } = [];

    public virtual JobOrder JobOrder { get; set; } = null!;
}