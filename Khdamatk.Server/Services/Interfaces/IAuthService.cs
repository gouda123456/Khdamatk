namespace Khdamatk.Server.Services.Interfaces;

public interface IAuthService : IService
{
    Task<resultBase> LoginAsync(LoginRequest userRequest, CancellationToken cancellationToken);
    Task<resultBase> RefreshToken(string RefreshToken);
    Task<resultBase> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken);
    Task<resultBase> ConfirmEmail(ConfirmEmailRequest request);
    Task<resultBase> ReSendConfirmationEmailAsync(ReSendConfirmationEmailRequest request);
}