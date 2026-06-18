namespace Khdamatk.Server.Data.Entities.Operations;

public abstract class OrderBase : BaseEntity
{
    public OrderStatus Status { get; set; } = OrderStatus.PendingPayment;
    public long? InvoiceId { get; set; }
    public string? InvoiceKey { get; set; }
    
    // === تفاصيل الطلب ===
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Range(1, 100_000_000)]
    public decimal Amount { get; set; } // المبلغ الكلي المتفق عليه (قد يتضمن ضرائب ورسوم)
}
