namespace Khdamatk.Server.Data.Entities.Interaction;


public class Dispute : BaseEntity
{
    // ربط كيان النزاع بطلب الخدمة (ServiceOrder)
    [Required]
    [ForeignKey(nameof(ServiceOrder))]
    public int ServiceOrderId { get; set; }
    public virtual ServiceOrder ServiceOrder { get; set; } = null!;

    //Raiser of the dispute (could be client or provider)
    [Required]
    [ForeignKey(nameof(Raiser))]
    public string RaiserId { get; set; }
    public virtual User Raiser { get; set; } = null!;

    //Target of the dispute (could be client or provider)
    [Required]
    [ForeignKey(nameof(Target))]
    public string TargetId { get; set; }
    public virtual User Target { get; set; } = null!;

    // معرّف المسؤول المُعيّن لإدارة النزاع (Admin ID - string)
    [ForeignKey(nameof(AdminReviewer))]
    public string? AdminReviewerId { get; set; } = null!;
    public virtual User? AdminReviewer { get; set; } = null!;


    // === 2. ربط المحادثات المنفصلة (Conversation ID - int) ===
    // كيان Dispute يتحكم بربط المحادثات (Unidirectional Relationship)

    // محادثة المسؤول مع الطرف الرافع
    [Required]
    [ForeignKey(nameof(RaiserConversation))]
    public int RaiserConversationId { get; set; }
    public virtual Conversation RaiserConversation { get; set; } = null!;

    // محادثة المسؤول مع الطرف المدعى عليه
    [Required]
    [ForeignKey(nameof(TargetConversation))]
    public int TargetConversationId { get; set; }
    public virtual Conversation TargetConversation { get; set; } = null!;



    // === 3. الخصائص المالية والحالة والقرار ===

    [Required]
    public DisputeStatus Status { get; set; } = DisputeStatus.Opened;

    [Required]
    public DisputeType Type { get; set; }

    // المبلغ المتنازع عليه (مهم جداً للقرار المالي)
    [Required]
    [Column(TypeName = "decimal(18, 2)")] // تحديد الدقة المالية
    public decimal AmountUnderDispute { get; set; }

    public string? ReasonDetails { get; set; } // تفاصيل السبب عند الرفع

    // القرار الإداري
    public string? FinalDecisionDetails { get; set; }

    // القبول (نستخدم bool? للسماح بقيمة null تعني 'لم يتم الرد بعد')
    public bool? IsDecisionAcceptedByRaiser { get; set; } // هل وافق الرافع؟
    public bool? IsDecisionAcceptedByTarget { get; set; } // هل وافق المدعى عليه؟

    public DateTime OpenedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ResolutionDate { get; set; }






}


public enum DisputeStatus
{
    Opened = 1,              // تم رفع النزاع للتو
    Assigned = 2,            // تم تعيين مسؤول مراجعة للنزاع
    UnderReview = 3,         // المسؤول يراجع الأدلة ويتواصل مع الأطراف
    AwaitingDecision = 4,    // انتهت المراجعة والقرار جاهز للإصدار
    AwaitingConfirmation = 5,// تم إصدار القرار وبانتظار موافقة الأطراف (Raiser & Target)
    Resolved = 6,            // تم قبول القرار من الطرفين أو تم التنفيذ القسري (Decision implemented)
    Closed = 7,              // النزاع مغلق بالكامل (تمت جميع الإجراءات المالية والإدارية)
    Cancelled = 8            // تم إلغاء النزاع من أحد الأطراف قبل تدخل الإدارة
}

public enum DisputeType
{
    QualityIssue = 1,       // الجودة لا تلبي المتطلبات
    LateDelivery = 2,       // تأخير في تسليم المشروع
    NonDelivery = 3,        // عدم تسليم أي شيء
    RefundClaim = 4,        // مطالبة برد مالي دون نزاع على الجودة
    ScopeViolation = 5,     // تجاوز نطاق العمل المتفق عليه
    Other = 99
}
