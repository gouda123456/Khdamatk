namespace Khdamatk.Server.Data.Configurations.CatalogConfig;

public class JobSkillRequirementConfiguration : IEntityTypeConfiguration<JobSkillRequirement>
{
    public void Configure(EntityTypeBuilder<JobSkillRequirement> builder)
    {
        // 1. تحديد الجدول (اختياري إذا كان الاسم مطابقاً)
        builder.ToTable("JobSkillRequirements");

        // 2. تعيين المفتاح المركب (Composite Primary Key)
        builder.HasKey(js => new { js.JobPostId, js.SkillId });

        // 3. إعداد العلاقات (Relationships)

        // علاقة مع JobPost: الوظيفة الواحدة لها مهارات متعددة
        builder.HasOne(js => js.JobPost)
            .WithMany(j => j.SkillRequirements)
            .HasForeignKey(js => js.JobPostId)
            .OnDelete(DeleteBehavior.Cascade); // عند حذف الوظيفة، تُحذف متطلبات المهارات الخاصة بها

        // علاقة مع Skill: المهارة الواحدة قد تُطلب في وظائف متعددة
        builder.HasOne(js => js.Skill)
            .WithMany() // نفترض أن كيان Skill لا يحتوي على Collection لمتطلبات الوظائف
            .HasForeignKey(js => js.SkillId)
            .OnDelete(DeleteBehavior.Restrict); // نمنع حذف مهارة (مثل C#) إذا كانت مطلوبة في وظائف حالية

        // 4. إعداد الخصائص الأخرى
        builder.Property(js => js.RequiredLevel)
            .IsRequired()
            .HasDefaultValue(SkillExperienceLevel.Beginner);
    }
}
