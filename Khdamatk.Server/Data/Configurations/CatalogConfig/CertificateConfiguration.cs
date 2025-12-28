
namespace Khdamatk.Server.Data.Configurations.CatalogConfig;

public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.HasOne(c => c.ServiceProviderProfile)
       .WithMany(sp => sp.Certificates)
       .HasForeignKey(c => c.ServiceProviderProfileId)
       .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.CertificateMedia) // HasOne: الشهادة لها صورة واحدة على الأكثر
               .WithOne() // WithOne: الصورة مرتبطة بشهادة واحدة على الأكثر
               .HasForeignKey<Certificate>(c => c.MediaId) // المفتاح الأجنبي (FK) موجود في جدول Certificate
               .OnDelete(DeleteBehavior.Restrict);
    }
}
