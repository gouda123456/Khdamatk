namespace Khdamatk.Server.Contracts.Authentications;

public record JwtToken(string Token, int ExpireAt);
public record JwtCliams(string UserId, string Email, List<string> Roles, List<string> Permissions);
public record RefreshToken(string Token, DateTime ExpireAt);

public record RefreshTokenRequest(string Token);

