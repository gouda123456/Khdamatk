namespace Khdamatk.Server.Data.Configurations.InteractionConfig;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        

        // 2. العلاقة مع العميل (Client)
        builder.HasOne(c => c.Client)
               .WithMany()
               .HasForeignKey(c => c.ClientId)
               .OnDelete(DeleteBehavior.Restrict);

        // 3. العلاقة مع مقدم الخدمة (Provider)
        builder.HasOne(c => c.Provider)
               .WithMany()
               .HasForeignKey(c => c.ProviderId)
               .OnDelete(DeleteBehavior.Restrict);

        

        // 5. تأكيد خصائص التدقيق من BaseEntity
        builder.Property(c => c.Createdat)
               .HasDefaultValueSql("GETUTCDATE()");
    }
}
