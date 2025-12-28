namespace Khdamatk.Server.Data.Entities.Operations;

// يفترض أنه يرث من BaseEntity
public class ServiceOrder : BaseEntity
{
    // Id و CreatedAt و IsActive موروثة من BaseEntity

    // === Foreign Keys ===

    [Required]
    [ForeignKey(nameof(User))]
    public string UserID { get; set; } = null!; // العميل (من قام بالطلب)

    [Required]
    [ForeignKey(nameof(Service))]
    public int ServiceID { get; set; } // الخدمة المطلوبة

    // ✅ هذا الرابط صحيح ويضمن السرعة في الاستعلام عن صاحب العمل
    [Required]
    [ForeignKey(nameof(ServiceProviderProfile))]
    public string ServiceProviderId { get; set; } = null!;

    // === تفاصيل الطلب ===
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Range(1, 100_000_000)]
    public decimal Amount { get; set; } // المبلغ الكلي المتفق عليه (قد يتضمن ضرائب ورسوم)

    [Required]
    public ServiceOrderStatus Status { get; set; } = ServiceOrderStatus.PendingPayment; // ✅ إضافة الحالة

    public DateTime? CompletionDate { get; set; } // تاريخ الإنجاز الفعلي

    [StringLength(1000)] // ملاحظات أو متطلبات إضافية
    public string? AdditionalDetails { get; set; }


    // === Navigation Properties ===
    public virtual User User { get; set; } = null!;
    public virtual Service Service { get; set; } = null!;
    public virtual ServiceProviderProfile ServiceProviderProfile { get; set; } = null!;

    // ✅ إضافة الروابط العكسية للكيانات التابعة لهذا الطلب
    public virtual Review? Review { get; set; } // علاقة 1-1 (تقييم واحد للطلب الواحد)
    public virtual PaymentTransaction? PaymentTransaction { get; set; } // علاقة 1-1
    public virtual ICollection<Message> Messages { get; set; } = []; // الرسائل الخاصة بهذا الطلب
}



// يمثل دورة حياة العمل على الخدمة
public enum ServiceOrderStatus
{
    // مرحلة ما قبل بدء العمل
    PendingPayment = 0,     // الطلب موجود ولكن في انتظار الدفع (بدء المعاملة)

    // مرحلة العمل
    Active = 1,             // تم الدفع بنجاح، العمل قيد التنفيذ
    UnderReview = 2,        // تم تسليم العمل، في انتظار مراجعة العميل

    // مرحلة الإغلاق (ناجح)
    Completed = 3,          // العميل وافق على التسليم، الطلب اكتمل بنجاح

    // مرحلة الإغلاق (فشل/إلغاء)
    CancelledByClient = 4,  // تم الإلغاء من قبل العميل (قد يتطلب استرداد)
    CancelledByProvider = 5,// تم الإلغاء من قبل مقدم الخدمة (قد يتطلب استرداد)
    Disputed = 6            // تم تحويل الطلب للنزاع (يتطلب تدخلاً إدارياً)
}