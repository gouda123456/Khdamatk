namespace Khdamatk.Server.Statics.Consts;

public static class JWTClaimsDefault
{
    public const string UserId = "UserId";
    public const string Email = "Email";
    public const string UserName = "UserName";
    public const string Role = ClaimTypes.Role;
    public const string Roles = "roles";
    public const string Permissions = PermissionsDefault.Type;
    public const string ID = JwtRegisteredClaimNames.Jti;
}
