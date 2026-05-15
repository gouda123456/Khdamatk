namespace Khdamatk.Server.Helper;

public static class UserExtension
{
    public static string? GetUserId(this ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(x => x.Type == JWTClaimsDefault.UserId)?.Value;
    }


    public static string LimitLength(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }
}
