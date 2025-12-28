namespace Khdamatk.Server.Data.Configurations.InteractionConfig;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        // 1. المفتاح الأساسي: Id موروث

        // 2. العلاقة مع المُرسِل (Sender)
        builder.HasOne(m => m.Sender)
               .WithMany()
               .HasForeignKey(m => m.SenderId)
               .OnDelete(DeleteBehavior.Restrict); // لا تحذف المستخدم إذا كان قد أرسل رسائل

        // 3. العلاقة مع المحادثة (Conversation)
        // يجب أن نعتمد على كيان Conversation لتحديد خاصية التنقل العكسية (مثل c.Messages)
        builder.HasOne(m => m.Conversation)
               .WithMany(c => c.Messages) // يجب أن يتم إضافة قائمة Messages إلى كيان Conversation
               .HasForeignKey(m => m.ConversationId)
               .OnDelete(DeleteBehavior.Cascade); // حذف الرسائل إذا حُذفت المحادثة

        // 4. تأكيد على الوقت
        builder.Property(m => m.Createdat) // أو الخاصية التي ورثتها
                .HasDefaultValueSql("GETUTCDATE()");
    }
}