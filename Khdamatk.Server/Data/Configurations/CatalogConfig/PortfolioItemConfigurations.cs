namespace Khdamatk.Server.Data.Configurations.CatalogConfig;

public class PortfolioItemConfigurations : IEntityTypeConfiguration<PortfolioItem>
{
    public void Configure(EntityTypeBuilder<PortfolioItem> builder)
    {
        builder.HasMany(p => p.ProjectMediaLinks)
               .WithOne(pm => pm.PortfolioItem)
               .HasForeignKey(pm => pm.PortfolioItemId);
    }
}
