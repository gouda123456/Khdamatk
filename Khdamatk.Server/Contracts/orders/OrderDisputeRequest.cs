namespace Khdamatk.Server.Contracts.Orders;

public class OrderDisputeRequest
{
    [Required]
    public int ServiceOrderId { get; set; }

    [Required]
    public DisputeType Type { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal AmountUnderDispute { get; set; }

    [Required]
    [StringLength(2000)]
    public string ReasonDetails { get; set; } = string.Empty;

    /// <summary>
    /// هل الرافع هو العميل (User) أم مقدم الخدمة؟
    /// </summary>
    [Required]
    public bool IsRaiserCustomer { get; set; }

    [Required]
    public int RaiserConversationId { get; set; }

    [Required]
    public int TargetConversationId { get; set; }
}

