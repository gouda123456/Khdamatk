namespace Khdamatk.Server.Data.Configurations.InteractionConfig;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        // 1. فرض أن كل ServiceOrder يجب أن يكون له محادثة واحدة (قيد فريد)
        builder.HasIndex(c => c.JobOrderId).IsUnique();
        builder.HasIndex(c => c.ServiceOrderId).IsUnique();

        // 2. العلاقة مع العميل (Client)
        builder.HasOne(c => c.Customer)
               .WithMany()
               .HasForeignKey(c => c.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);

        // 3. العلاقة مع مقدم الخدمة (Provider)
        builder.HasOne(c => c.Provider)
               .WithMany()
               .HasForeignKey(c => c.ProviderId)
               .OnDelete(DeleteBehavior.Restrict);

        

        builder.HasOne(c => c.JobOrder)
               .WithOne() // علاقة 1-1 (منطقية)
               .HasForeignKey<Conversation>(c => c.JobOrderId)
               .OnDelete(DeleteBehavior.Cascade); // حذف المحادثة عند حذف الطلب


        // 5. تأكيد خصائص التدقيق من BaseEntity
        builder.Property(c => c.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");
    }
}
