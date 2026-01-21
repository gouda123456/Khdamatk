namespace Khdamatk.Server.Data.Configurations.IdentityConfig;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(DefaultUsersData.Admin, DefaultUsersData.Member);

        builder.HasOne(sp => sp.ProfilePicture)
           .WithOne()
           .HasForeignKey<User>(sp => sp.ProfilePicture)
           .OnDelete(DeleteBehavior.SetNull);
    }
}
