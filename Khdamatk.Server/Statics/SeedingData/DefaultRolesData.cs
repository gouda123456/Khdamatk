namespace Khdamatk.Server.Statics.SeedingData;

public static class DefaultRolesData
{
    public static Role Admin { get; set; } = new()
    {
        Id = "d74ddd14-6340-4840-95c2-db12554843e5",
        Name = RolesStrings.Admin,
        NormalizedName = RolesStrings.Admin.ToUpper(),
        ConcurrencyStamp = "1"
    };

    public static Role Member { get; set; } = new()
    {
        Id = "e3b0c442-98fc-4c2a-8b2e-3f9d6f1a1a66",
        Name = RolesStrings.Member,
        NormalizedName = RolesStrings.Member.ToUpper(),
        ConcurrencyStamp = "2"
    };

    public static Role ServiceProvider { get; set; } = new()
    {
        Id = "9f86d081-884c-4f9b-bb5f-7b4b5f9c2a77",
        Name = RolesStrings.ServiceProvider,
        NormalizedName = RolesStrings.ServiceProvider.ToUpper(),
        ConcurrencyStamp = "3"
    };
}
