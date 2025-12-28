namespace Khdamatk.Server.Services.Interfaces;

public interface ITokensService : IService
{
    Task<JwtToken> GetJwtToken(User user);
    string? ValidateToken(string token);
    RefreshToken RefreshToken(User user);
}
