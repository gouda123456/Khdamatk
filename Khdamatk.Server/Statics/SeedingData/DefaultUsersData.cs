namespace Khdamatk.Server.Statics.SeedingData;

public static class DefaultUsersData
{
    public const string AdminId = "b74ddd14-6340-4840-95c2-db12554843e5";
    public static  User Admin { get; set; } = new()
    {
        Id = AdminId,
        UserName = "Admin",
        NormalizedUserName = "ADMIN",
        Email = "admin@admin.com",
        NormalizedEmail ="ADMIN@ADMIN.COM",
        EmailConfirmed = true,
        LockoutEnabled = false,
        SecurityStamp = "AQAAAAEAACcQAAAAEPG6fy3Z6k0rG+1bF5uV1r+H1l5H3g==",
        PasswordHash = "AQAAAAIAAYagAAAAEDTQ2MJwg8G7sNyX4vUuSzK8tnhxhzDsZpWvc8jt01mVjPURiVuM4G80qBh84/zwgA==",//Password: Admin@123
        DateOfBirth = new DateTime(1990, 1, 1),
        ConcurrencyStamp = "abd61835-2981-4f28-9a87-3992de5a2460"
    };
    
    public static User Member { get; set; } = new()
    {
        Id = "b74ddd14-6340-4840-95c2-db12554843eslkna5",
        UserName = "User",
        NormalizedUserName = "User",
        Email = "user@user.com",
        NormalizedEmail = "USER@USER.COM",
        EmailConfirmed = true,
        LockoutEnabled = false,
        SecurityStamp = "AQAAAAEAACcQAAAAEPG6fy3Z6k0rG+1bF5uV1r+H1l5H3g==nn",
        PasswordHash = "AQAAAAIAAYagAAAAEOu4XNo4PNSoxFgFM7DdE+/Fc3QVNdoUJ+LGU0bT0tlSTvDC0PRb9ofBLridqu7fkQ==",//Password: User@123
        DateOfBirth = new DateTime(1990, 1, 1),
        ConcurrencyStamp = "c84d599b-2289-47a5-81ed-061fbf16ce50"
    };
}