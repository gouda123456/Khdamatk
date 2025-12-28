namespace Khdamatk.Server.Data.Entities.Financial;


public class PaymentTransaction
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(ServiceOrder))]
    public int OrderId { get; set; }

    // === المبالغ المالية ===

    [Required]
    [Column(TypeName = "decimal(18, 2)")] // دقة عالية للأمور المالية
    public decimal Amount { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal PlatformFee { get; set; } // رسوم المنصة

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal NetPayout { get; set; } // المبلغ الصافي للمزود بعد خصم الرسوم


    


    // === تفاصيل الدفع والحالة ===

    [Required]
    public CurrencyCode Currency { get; set; } = CurrencyCode.USD;

    [Required]
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    [Required]
    public PaymentGateway GatewayUsed { get; set; } = PaymentGateway.Stripe;

    

    

    /// <summary>
    /// رقم المعاملة المعطى من قبل بوابة الدفع الخارجية (مثل PaymentIntent ID, Fawry Reference).
    /// </summary>
    [MaxLength(256)]
    public string? GatewayReferenceId { get; set; }


    


    /// <summary>
    /// المفتاح الخارجي لكيان CreditCard إذا تم استخدام بطاقة محفوظة.
    /// يُستخدم فقط للدفع بالبطاقات المرمزة.
    /// </summary>
    [ForeignKey(nameof(TokenizedCard))]
    public int? TokenizedCreditCardId { get; set; }



    // === Navigation Properties ===
    public virtual ServiceOrder ServiceOrder { get; set; } = null!;
    public virtual CreditCard? TokenizedCard { get; set; }

}


public enum TransactionStatus
{
    Pending,        // 0 - في انتظار بدء المعاملة/الدفع
    Waited,         // 1 - في انتظار التأكيد من بوابة الدفع الخارجية (مثل Webhook من Fawry/PayMob)
    Completed,      // 2 - تم الدفع بنجاح
    Failed,         // 3 - فشلت المعاملة
    RefundInitiated,// 4 - تم بدء عملية الاسترداد
    Refunded        // 5 - تم الاسترداد بنجاح
}

public enum PaymentGateway
{
    Stripe = 1,
    Fawry = 2,
    InstaPay = 3,
    LocalBankTransfer = 4,
    // ...
}

public enum CurrencyCode
{
    // 0. لا شيء (لتجنب تعيين قيمة افتراضية خاطئة)
    None = 0,

    // 1. العملات المحلية (الرئيسية)
    EGP = 1,  // الجنيه المصري

    // 2. العملات الدولية والخليجية
    USD = 2,  // الدولار الأمريكي
    EUR = 3,  // اليورو
    SAR = 4,  // الريال السعودي
    AED = 5,  // الدرهم الإماراتي
    KWD = 6,  // الدينار الكويتي
              // ملاحظة: يُفضل استخدام رمز العملة (Code) كاسم للتعداد
}