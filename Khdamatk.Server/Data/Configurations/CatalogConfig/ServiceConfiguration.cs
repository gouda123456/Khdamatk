namespace Khdamatk.Server.Data.Configurations.CatalogConfig;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        //TODO: ... إعدادات الـ Primary Key ...

        // إعداد مجموعة الـ Media كـ Owned Type
        builder.HasMany(s => s.MediaGalleryLinks) // Collection يشير لكيان الربط
               .WithOne(sm => sm.Service)
               .HasForeignKey(sm => sm.ServiceId);

        // 2. تكوين علاقة One-to-One مع الصورة الرئيسية (MainImage) (كما تم في Certificate)
        builder.HasOne(s => s.MainImage)
               .WithOne()
               .HasForeignKey<Service>(s => s.MainMediaId)
               .OnDelete(DeleteBehavior.SetNull); // SetNull مناسب للصورة الاختيارية
    }
}