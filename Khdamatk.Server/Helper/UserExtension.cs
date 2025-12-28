namespace Khdamatk.Server.Helper;

public static class UserExtension
{
    public static string? GetUserId(this ClaimsPrincipal user)
    {
        return user.Claims.First(x => x.Type == JWTClaimsDefault.UserId).Value;
    }
}
