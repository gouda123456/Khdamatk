
namespace Khdamatk.Server.Data.Configurations.IdentityConfig;

public class ServiceProviderProfileConfigurations : IEntityTypeConfiguration<ServiceProviderProfile>
{
    public void Configure(EntityTypeBuilder<ServiceProviderProfile> builder)
    {
        // 1. المفتاح الأساسي والعلاقة مع المستخدم
        builder.HasKey(p => p.UserId);

        builder.HasOne(p => p.User)
               .WithOne() // افتراضاً أن المستخدم لديه بروفايل واحد كمقدم خدمة
               .HasForeignKey<ServiceProviderProfile>(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        // 2. ضبط الحقول الأساسية
        builder.Property(p => p.JobTitle)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(p => p.Bio)
               .IsRequired()
               .HasMaxLength(1000);



        // 4. ضبط علاقة الـ Reviews داخل الـ Profile
        builder.HasMany(p => p.Reviews) // تأكد من إضافة هذه القائمة في كلاس الـ Profile
               .WithOne(r => r.ServiceProvider)
               .HasForeignKey(r => r.ServiceProviderId)
               .OnDelete(DeleteBehavior.NoAction); // نستخدم NoAction لتجنب تعارض الـ Cascade Path
        // علاقة مقدم الخدمة بالشهادات (One-to-Many)
        builder.HasMany(p => p.Certificates)
               .WithOne(c => c.ServiceProviderProfile)
               .HasForeignKey(c => c.ServiceProviderProfileId)
               .OnDelete(DeleteBehavior.Cascade);

        // علاقة مقدم الخدمة بالعروض المقدمة (One-to-Many)
        builder.HasMany(p => p.JobOffers)
               .WithOne(o => o.ProviderProfile)
               .HasForeignKey(o => o.ProviderProfileId)
               .OnDelete(DeleteBehavior.Restrict); // نستخدم Restrict لضمان عدم ضياع تاريخ العروض بسهولة

        // علاقة مقدم الخدمة بالخدمات المعروضة (One-to-Many)
        builder.HasMany(p => p.Services)
               .WithOne(s => s.ServiceProviderProfile)
               .HasForeignKey(s => s.ServiceProviderProfileId)
               .OnDelete(DeleteBehavior.Cascade);

        // 5. الفهارس (Indexes) للأداء
        builder.HasIndex(p => p.IsActive);
        builder.HasIndex(p => p.IsAvailable);

    }
}
