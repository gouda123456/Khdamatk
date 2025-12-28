namespace Khdamatk.Server.Data.Configurations.IdentityConfig;

public class RolesClaimsConfigurations : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        var Permissions = PermissionsDefault.GetAllPermissions();
        var adminClaims = new List<IdentityRoleClaim<string>>();

        for (int i = 0; i < Permissions.Count; i++)
        {
            adminClaims.Add(new IdentityRoleClaim<string>
            {
                Id = i + 1,
                RoleId = DefaultRolesData.Admin.Id,
                ClaimType = PermissionsDefault.Type,
                ClaimValue = Permissions[i]
            });

        }
        builder.HasData(adminClaims);


        List<string> userClaims = [
            PermissionsDefault.WeatherForecast.View,
            PermissionsDefault.Authentications.View,
            PermissionsDefault.Users.View
        ];

        var normalUserClaims = new List<IdentityRoleClaim<string>>();

        for (int i = 0; i < userClaims.Count; i++)
        {
            normalUserClaims.Add(new IdentityRoleClaim<string>
            {
                Id = Permissions.Count + i + 1,
                RoleId = DefaultRolesData.Member.Id,
                ClaimType = PermissionsDefault.Type,
                ClaimValue = userClaims[i]
            });
        }
        builder.HasData(normalUserClaims);

    }
}
