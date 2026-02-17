namespace Khdamatk.Server.Services.Interfaces;

public interface IAuthService : IService
{
    Task<resultBase> LoginAsync(LoginRequest userRequest, CancellationToken cancellationToken);
    Task<resultBase> RefreshToken(string RefreshToken);
    Task<resultBase> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken);
    Task<resultBase> ConfirmEmail(ConfirmEmailRequest request);
    Task<resultBase> ReSendConfirmationEmailAsync(ReSendConfirmationEmailRequest request);
    Task<resultBase> ForgetPassword(string Email);
    Task<resultBase> VerifyCodeAsync(VerifyRestPasswordCodeRequest request,CancellationToken cancellationToken = default);
    Task<resultBase> SetPasswordAsync(SetPasswordRequest request);
    Task<resultBase> ForgetPasswordAsync(string email, CancellationToken cancellationToken = default);
}