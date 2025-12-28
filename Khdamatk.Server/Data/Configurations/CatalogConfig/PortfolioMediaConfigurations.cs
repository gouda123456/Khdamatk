namespace Khdamatk.Server.Data.Configurations.CatalogConfig;

public class PortfolioMediaConfigurations : IEntityTypeConfiguration<PortfolioMedia>
{
    public void Configure(EntityTypeBuilder<PortfolioMedia> builder)
    {
        builder.HasKey(sm => new { sm.PortfolioItemId, sm.MediaId });
    }
}

