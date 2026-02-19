namespace Khdamatk.Server.Services.Interfaces;

public interface IAuthService : IService
{
    Task<resultBase> LoginAsync(LoginRequest userRequest, CancellationToken cancellationToken);
    Task<resultBase> RefreshTokenAsync(string RefreshToken, CancellationToken cancellationToken = default);
    Task<resultBase> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken);
    Task<resultBase> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> ReSendConfirmationEmailAsync(ReSendConfirmationEmailRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> VerifyCodeAsync(VerifyRestPasswordCodeRequest request,CancellationToken cancellationToken = default);
    Task<resultBase> SetPasswordAsync(SetPasswordRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> ForgetPasswordAsync(string email, CancellationToken cancellationToken = default);
}