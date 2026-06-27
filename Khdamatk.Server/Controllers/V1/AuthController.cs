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
        (await authService.RefreshTokenAsync(refreshToken.Token)).Respond();


    [HttpGet("Confirm")]
    public async Task<IActionResult> Confirm([FromQuery] ConfirmEmailRequest request) =>
        (await authService.ConfirmEmailAsync(request)).Respond();

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

    [HttpGet("Profile/{UserId}")]
    public async Task<IActionResult> GetProfile([FromServices] Database db, [FromRoute] string UserId, CancellationToken cancellationToken)
    {
        var user = await db.Users.FirstOrDefaultAsync(x => x.Id == UserId, cancellationToken);       
        if (user == null)
                return NotFound();

        var HiringHistoryJobOrder = db.JobOrders.Include(f => f.ServiceProviderProfile).ThenInclude(u => u.User).ThenInclude(m=>m.ProfilePicture)
            .Select(j => new UserRpfileHiringHistory(
            (j.ServiceProviderProfile.User.ProfilePicture != null) ? j.ServiceProviderProfile.User.ProfilePicture.DownloadFileAsyncPathVersion() : "https://tse4.mm.bing.net/th/id/OIP.7FsDgas0kcH0W1ajb1rZEgHaHa?r=0&cb=thfc1falcon3&rs=1&pid=ImgDetMain&o=7&rm=3",
            j.Job.Title,
            j.Amount,
            j.Status,
            j.CreatedAt
            )
        ).ToList();

        var HiringHistoryServiceOrders = db.ServiceOrders.Select(j => new UserRpfileHiringHistory
        (
            (j.ServiceProviderProfile.User.ProfilePicture != null) ?j.ServiceProviderProfile.User.ProfilePicture.DownloadFileAsyncPathVersion():"https://tse4.mm.bing.net/th/id/OIP.7FsDgas0kcH0W1ajb1rZEgHaHa?r=0&cb=thfc1falcon3&rs=1&pid=ImgDetMain&o=7&rm=3",
            j.Service.Title,
            j.Amount,
            j.Status,
            j.CreatedAt
            )
        ).ToList();

        var TotalSpendBalance = HiringHistoryServiceOrders.Sum(s => s.Amount);

            var UserRpfileTransaction = db.JobOrders.Select(j => new UserRpfileTransaction(
                j.Job.Title,
                j.Amount,
                j.CreatedAt
                )
            ).ToList();

        UserRpfileTransaction.AddRange(db.ServiceOrders.Select(j => new UserRpfileTransaction(
                j.Service.Title,
                j.Amount,
                j.CreatedAt
                )
            ).ToList());

        HiringHistoryJobOrder.AddRange(HiringHistoryServiceOrders);

        var response = new UserProfileResponse(
            user.Id,
            user.FullName,
            user.Email,
            $"{user.VerificationData.Country},{user.VerificationData.City}",
            user.CreatedAt.ToString("d"),
            user.CalculatedRate,
            user.JobPosts.Count,
            user.JobPosts.Where(j => j.Status == JobPostStatus.Completed).Count(),
            user.Amount,
            user.ProfilePicture?.DownloadFileAsyncPathVersion() ?? "https://tse4.mm.bing.net/th/id/OIP.7FsDgas0kcH0W1ajb1rZEgHaHa?r=0&cb=thfc1falcon3&rs=1&pid=ImgDetMain&o=7&rm=3",
            user.JobPosts.Select(j => new UserRpfilePostedJobs(
                j.Category.Icon,
                j.Category.Name,
                j.Title,
                j.BudgetMax,
                j.Status
                )).ToList(),
            HiringHistoryJobOrder,
            new VerifyCardUserProfile(
                user.EmailConfirmed,
                user.PhoneNumberConfirmed,
                user.IsVerified,
                user.IsTrustedByAdmin
                ),
            new UserRpfileFinancial(
                user.Amount,
                TotalSpendBalance,
                UserRpfileTransaction
                )

            );


        return Ok(response);

    }


}
//Giggo343@gmail.com
//giggo343@gmail.com
//Giggo343@