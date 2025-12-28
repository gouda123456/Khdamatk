namespace Khdamatk.Server.Data.Configurations.IdentityConfig;

public class RoleConfigurations : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(DefaultRolesData.Admin, DefaultRolesData.Member);

    }
}