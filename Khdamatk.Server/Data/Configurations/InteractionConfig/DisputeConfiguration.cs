
using Microsoft.EntityFrameworkCore;

namespace Khdamatk.Server.Data.Configurations.InteractionConfig;

public class DisputeConfiguration : IEntityTypeConfiguration<Dispute>
{
    public void Configure(EntityTypeBuilder<Dispute> builder)
    {
        // مثال للتكوين الإلزامي في DbContext.OnModelCreating
        builder
            .HasOne(d => d.Raiser)
            .WithMany() // يمكن أن تكون WithMany("RaiserDisputes") في كيان User
            .HasForeignKey(d => d.RaiserId)
            .OnDelete(DeleteBehavior.Restrict); // يفضل استخدام Restrict لتجنب الحذف المتسلسل

        builder
            .HasOne(d => d.Target)
            .WithMany()
            .HasForeignKey(d => d.TargetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(d => d.AdminReviewer)
            .WithMany()
            .HasForeignKey(d => d.AdminReviewerId)
            .OnDelete(DeleteBehavior.Restrict);



        // التكوين الإضافي المطلوب لكيان Dispute:

        builder.HasOne(d => d.RaiserConversation)
            .WithOne() // أو WithOne(c => c.RaiserDisputeLink)
            .HasForeignKey<Dispute>(d => d.RaiserConversationId)
            .OnDelete(DeleteBehavior.Restrict); // لمنع التعارضات مع العلاقات الأخرى

        builder.HasOne(d => d.TargetConversation)
            .WithOne()
            .HasForeignKey<Dispute>(d => d.TargetConversationId)
            .OnDelete(DeleteBehavior.Restrict);




    }
}
