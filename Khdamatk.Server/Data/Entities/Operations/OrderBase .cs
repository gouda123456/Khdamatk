namespace Khdamatk.Server.Data.Entities.Operations;

public abstract class OrderBase : BaseEntity
{
    public OrderStatus Status { get; set; } = OrderStatus.PendingPayment;
    public long InvoiceId { get; set; }
    public string InvoiceKey { get; set; } = null!;
    // === تفاصيل الطلب ===
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Range(1, 100_000_000)]
    public decimal Amount { get; set; } // المبلغ الكلي المتفق عليه (قد يتضمن ضرائب ورسوم)
    public virtual PaymentTransaction? PaymentTransaction { get; set; }
    public virtual Review? Review { get; set; }
    public virtual ICollection<Message> Messages { get; set; } = []; // الرسائل الخاصة بهذا الطلب
    public virtual ICollection<Media> MediaAttachments { get; set; } = []; // المرفقات (صور، ملفات، إلخ) 
}