namespace Khdamatk.Server.Data.Entities.Operations;

public class JobDeliverable : BaseEntity
{
    public int JobOrderId { get; set; }
    public string Description { get; set; }
    public string? FileUrl { get; set; } // رابط الملف المرفوع

    public virtual JobOrder JobOrder { get; set; } = null!;
}