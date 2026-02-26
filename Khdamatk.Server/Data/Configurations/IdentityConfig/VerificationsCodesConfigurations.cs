namespace Khdamatk.Server.Data.Configurations.IdentityConfig;

public class VerificationsCodesConfigurations : IEntityTypeConfiguration<VerificationsCodes>
{
    public void Configure(EntityTypeBuilder<VerificationsCodes> builder)
    {
        builder.HasOne(vc => vc.User)
            .WithMany(u => u.VerificationsCodes)
            .HasForeignKey(vc => vc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
