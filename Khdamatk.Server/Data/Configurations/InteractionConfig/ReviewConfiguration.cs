
namespace Khdamatk.Server.Data.Configurations.InteractionConfig;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        
        // 1. العلاقة مع ServiceOrder (قوية، يمكن أن تكون Cascade)
        builder.HasOne(r => r.ServiceOrder)
               .WithOne() // افتراضياً، المراجعة الواحدة لطلب خدمة واحد
               .HasForeignKey<Review>(r => r.OrderId)
               .OnDelete(DeleteBehavior.Cascade); // إذا حُذف الطلب، تُحذف المراجعة

        // 2. العلاقة مع User (كمراجع): يجب كسر السلسلة هنا
        builder.HasOne(r => r.Reviewer)
               .WithMany()
               .HasForeignKey(r => r.ReviewerId)
               .OnDelete(DeleteBehavior.Restrict); // يمنع حذف المستخدم إذا كان لديه مراجعات

        
    }
}
