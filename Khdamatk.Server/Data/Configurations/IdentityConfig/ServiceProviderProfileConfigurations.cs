
namespace Khdamatk.Server.Data.Configurations.IdentityConfig;

public class ServiceProviderProfileConfigurations : IEntityTypeConfiguration<ServiceProviderProfile>
{
    public void Configure(EntityTypeBuilder<ServiceProviderProfile> builder)
    {
        builder.HasOne(sp => sp.ProfilePicture)
            .WithOne()
            .HasForeignKey<ServiceProviderProfile>(sp => sp.ProfilePictureId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
