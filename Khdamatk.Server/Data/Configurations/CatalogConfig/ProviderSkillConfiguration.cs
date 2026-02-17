namespace Khdamatk.Server.Data.Configurations.CatalogConfig;

public class ProviderSkillConfiguration : IEntityTypeConfiguration<ProviderSkill>
{
    public void Configure(EntityTypeBuilder<ProviderSkill> builder)
    {
        // 1. تحديد اسم الجدول
        builder.ToTable("ProviderSkills");

        // 2. تعيين المفتاح المركب (Composite Primary Key)
        builder.HasKey(ps => new { ps.ServiceProviderProfileId, ps.SkillId });

        // 3. إعداد العلاقات (Relationships)

        // علاقة مع الملف الشخصي لمقدم الخدمة
        builder.HasOne(ps => ps.Profile)
            .WithMany(p => p.Skills) // تأكد أن Profile يحتوي على ICollection<ProviderSkill>
            .HasForeignKey(ps => ps.ServiceProviderProfileId)
            .OnDelete(DeleteBehavior.Cascade); // إذا حُذف الملف الشخصي، تُحذف مهاراته تلقائياً

        // علاقة مع جدول المهارات المركزي
        builder.HasOne(ps => ps.Skill)
            .WithMany()
            .HasForeignKey(ps => ps.SkillId)
            .OnDelete(DeleteBehavior.Restrict); // نمنع حذف المهارة الأصلية إذا كان هناك فريلانسر يستخدمها

        // 4. إعداد الخصائص
        builder.Property(ps => ps.MyLevel)
            .IsRequired()
            .HasDefaultValue(SkillExperienceLevel.Beginner);

        // إضافة Index للسرعة عند البحث عن مقدمي خدمة بمهارة معينة
        builder.HasIndex(ps => ps.SkillId);
    }
}
