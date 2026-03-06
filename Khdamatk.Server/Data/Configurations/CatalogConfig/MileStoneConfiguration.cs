namespace Khdamatk.Server.Data.Configurations.CatalogConfig;

public class MileStoneConfiguration : IEntityTypeConfiguration<MileStone>
{
    public void Configure(EntityTypeBuilder<MileStone> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.Title);
    }
}
