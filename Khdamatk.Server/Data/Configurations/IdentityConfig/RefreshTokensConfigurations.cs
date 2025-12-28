namespace Khdamatk.Server.Data.Configurations.IdentityConfig;

public class RefreshTokensConfigurations : IEntityTypeConfiguration<RefreshTokens>
{
    public void Configure(EntityTypeBuilder<RefreshTokens> builder)
    {
        builder.Property(rt => rt.IsUsed).HasDefaultValue(false);

        builder.Property(rt => rt.IsDeleted).HasDefaultValue(false);

        
        builder.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(rt => rt.Token).IsUnique();

    }
}
