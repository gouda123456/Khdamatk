
namespace Khdamatk.Server.Data.Configurations.CatalogConfig;

public class ServiceMediaConfigurations : IEntityTypeConfiguration<ServiceMedia>
{
    public void Configure(EntityTypeBuilder<ServiceMedia> builder)
    {
        builder.HasKey(sm => new { sm.ServiceId, sm.MediaId });
    }
}
