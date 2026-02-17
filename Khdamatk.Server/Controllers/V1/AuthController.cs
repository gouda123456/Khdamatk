using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("[controller]")]
[ApiController]

public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService authService = authService;

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request, cancellationToken);
        return result.Respond();
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(request, cancellationToken);
        return result.Respond();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshToken) =>
        (await authService.RefreshToken(refreshToken.Token)).Respond();


    [HttpGet("Confirm")]
    public async Task<IActionResult> Confirm([FromQuery] ConfirmEmailRequest request) =>
        (await authService.ConfirmEmail(request)).Respond();

    [HttpPost("resend-confirmation-email")]
    public async Task<IActionResult> ReSendConfirmationEmail(ReSendConfirmationEmailRequest request) =>
        (await authService.ReSendConfirmationEmailAsync(request)).Respond();

    [HttpPost("set-password")]
    public async Task<IActionResult> SetPassword(SetPasswordRequest request) =>
        (await authService.SetPasswordAsync(request)).Respond();

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword(string email) =>
        (await authService.ForgetPasswordAsync(email)).Respond();

       [HttpPost("verify-code")]
       public async Task<IActionResult> VerifyCode(VerifyRestPasswordCodeRequest request, CancellationToken cancellationToken) =>
        (await authService.VerifyCodeAsync(request, cancellationToken)).Respond();

}
