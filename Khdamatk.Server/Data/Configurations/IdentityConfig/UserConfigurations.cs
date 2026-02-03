namespace Khdamatk.Server.Data.Configurations.IdentityConfig;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(DefaultUsersData.Admin, DefaultUsersData.Member);

        builder.HasOne(u => u.ProfilePicture)
            .WithOne()
            .HasForeignKey<User>(u => u.ProfilePictureId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
