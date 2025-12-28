namespace Khdamatk.Server.Data.Configurations.IdentityConfig;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(
            new IdentityUserRole<string>
            {
                RoleId = DefaultRolesData.Admin.Id,
                UserId = DefaultUsersData.AdminId
            }
            ,
             new IdentityUserRole<string>
             {
                 RoleId = DefaultRolesData.Member.Id,
                 UserId = DefaultUsersData.AdminId
             },
             new IdentityUserRole<string>
             {
                 RoleId = DefaultRolesData.Member.Id,
                 UserId = DefaultUsersData.Member.Id
             }
        );
    }
}