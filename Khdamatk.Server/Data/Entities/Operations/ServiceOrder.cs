namespace Khdamatk.Server.Data.Entities.Operations;

// يفترض أنه يرث من BaseEntity
public class ServiceOrder : OrderBase
{
    // Id و CreatedAt و IsActive موروثة من BaseEntity

    // === Foreign Keys ===

    [Required]
    [ForeignKey(nameof(User))]
    public string UserID { get; set; } = null!; // العميل (من قام بالطلب)

    [Required]
    public int ServiceID { get; set; } // الخدمة المطلوبة

    // ✅ هذا الرابط صحيح ويضمن السرعة في الاستعلام عن صاحب العمل
    [Required]
    [ForeignKey(nameof(ServiceProviderProfile))]
    public string ServiceProviderId { get; set; } = null!;

    

    

    

    public DateTime? CompletionDate { get; set; } // تاريخ الإنجاز الفعلي

    [StringLength(1000)] // ملاحظات أو متطلبات إضافية
    public string? AdditionalDetails { get; set; }

    


    // === Navigation Properties ===
    public virtual User User { get; set; } = null!;
    public virtual Service Service { get; set; } = null!;

    public virtual ServiceProviderProfile ServiceProviderProfile { get; set; } = null!;
    public virtual Conversation Conversation { get; set; } = null!;


    // ✅ إضافة الروابط العكسية للكيانات التابعة لهذا الطلب
    
    
    
}



// يمثل دورة حياة العمل على الخدمة
public enum OrderStatus
{

    // مرحلة ما قبل بدء العمل
    Pending,             // الطلب تم إنشاؤه ولكن لم يتم اتخاذ أي إجراء بعد

    PendingPayment ,     // الطلب موجود ولكن في انتظار الدفع (بدء المعاملة)

    // مرحلة العمل
    Active ,             // تم الدفع بنجاح، العمل قيد التنفيذ
    UnderReview ,        // تم تسليم العمل، في انتظار مراجعة العميل

    // مرحلة الإغلاق (ناجح)
    Completed ,          // العميل وافق على التسليم، الطلب اكتمل بنجاح

    // مرحلة الإغلاق (فشل/إلغاء)
    CancelledByClient ,  // تم الإلغاء من قبل العميل (قد يتطلب استرداد)
    CancelledByProvider ,   // تم الإلغاء من قبل مقدم الخدمة (قد يتطلب استرداد)
    Disputed             // تم تحويل الطلب للنزاع (يتطلب تدخلاً إدارياً)
}