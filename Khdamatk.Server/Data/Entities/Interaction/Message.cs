namespace Khdamatk.Server.Data.Entities.Interaction;

public class Message : BaseEntity // Id و CreatedAt (مُعدّلة من SentAt) موروثتان
{
    // === 1. علاقة المحادثة (الأساس) ===

    [Required]
    [ForeignKey(nameof(Conversation))]
    public int ConversationId { get; set; }
    public virtual Conversation Conversation { get; set; } = null!;


    // === 2. علاقة المرسل (طرف واحد فقط) ===

    [Required]
    [ForeignKey(nameof(Sender))]
    public string SenderId { get; set; } = null!; // من أرسل الرسالة
    public virtual User Sender { get; set; } = null!;

    // ❌ تم حذف ReceiverId لأنه يجب أن يتم تحديده في Conversation

    // === 3. محتوى الرسالة وتفاصيلها ===

    [Required]
    [StringLength(2000, MinimumLength = 1)]
    public string Content { get; set; } = string.Empty;

    public bool IsRead { get; set; } = false;

    // يمكن إضافة IsEdited أو Deleted هنا إذا لزم الأمر

    // TODO: 💡 ملاحظة: إذا كنت بحاجة للمرفقات، استخدم كيان Media أو جدول ربط M:N.
    // public int? AttachmentMediaId { get; set; }
    // public virtual Media? AttachmentMedia { get; set; }
}