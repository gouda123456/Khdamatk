using Khdamatk.Server.Helper;
using Khdamatk.Server.Statics.Errors;
using Khdamatk.Server.Statics.MessagesRespond;
using Microsoft.AspNetCore.WebUtilities;

namespace Khdamatk.Server.Services.Implementations;

public class AuthService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    ITokensService tokensService,
    Database db,
    IEmailHelper emailHelper,
    IOptions<ClientSetting> clientSettings
    ) : IAuthService
{
    private readonly UserManager<User> userManager = userManager;
    private readonly SignInManager<User> signInManager = signInManager;
    private readonly ITokensService tokensService = tokensService;
    private readonly Database db = db;
    private readonly IEmailHelper emailHelper = emailHelper;
    private readonly ClientSetting clientSettings = clientSettings.Value;

    public async Task<resultBase> LoginAsync(LoginRequest userRequest, CancellationToken cancellationToken = default)
    {
        //check if user exists
        var user = await userManager.FindByEmailAsync(userRequest.Email);

        if (user is null)
        {
            return Failure(404, UserErrors.Title, UserErrors.Message, UserErrors.UserNotFound);
        }

        //check password and Email confirmation
        var result = await signInManager.CheckPasswordSignInAsync(user!, userRequest.Password, false);

        if (result.Succeeded)
        {
            //generate jwt token
            JwtToken jwtToken = await tokensService.GetJwtToken(user!);

            //generate Refresh token
            var refreshToken = tokensService.RefreshToken(user!);



            //send Login response
            return Success(200, new LoginResponse(jwtToken, refreshToken));
        }
        else
        {
            return Failure(401, UserErrors.Title, UserErrors.Message, UserErrors.InvalidPassword);
        }

    }


    // (email , password) => token, token 

    public async Task<resultBase> RefreshToken(string RefreshToken)
    {

        // get user from token
        var user = await userManager.Users
            .Include(r => r.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == RefreshToken));

        if (user is null)
            return Failure(StatusCodes.Status404NotFound, UserErrors.UserNotFound);

        //check if token is active and it its not check if its used for security 
        var lasttoken = user.RefreshTokens.SingleOrDefault(r => r.Token == RefreshToken);

        if (lasttoken is null)
            return Failure(StatusCodes.Status404NotFound, UserErrors.RefreshTokenDoesNotExists);

        if (!lasttoken!.IsActive)
        {
            if (lasttoken.IsUsed)
            {
                user.RefreshTokens.ForEach(r =>
                {
                    r.IsDeleted = true;
                    r.RevokedAt = DateTime.UtcNow;
                }
                );
                await db.SaveChangesAsync();
                return Failure(StatusCodes.Status404NotFound, UserErrors.RefreshTokenDoesNotExists); //To deceive the hacker 
            }
            else
            {
                return Failure(StatusCodes.Status400BadRequest, UserErrors.InvalidRefreshToken);
            }

        }


        //revoke token

        lasttoken.IsUsed = true;
        lasttoken.RevokedAt = DateTime.UtcNow;

        // generate new jwt and refresh token
        var newJwt = await tokensService.GetJwtToken(user);
        var newRefreshToken = tokensService.RefreshToken(user);

        await db.SaveChangesAsync();

        return Success(StatusCodes.Status200OK,nameof(UserMessagesRespond.LoginSuccessful), UserMessagesRespond.LoginSuccessful, new LoginResponse(newJwt, newRefreshToken));

    }

    public async Task<resultBase> RegisterAsync(RegisterRequest Request, CancellationToken cancellationToken)
    {
        //check Duplicate Email
        var existingUser = await userManager.FindByEmailAsync(Request.Email);

        if (existingUser != null)
        {
            return Failure(StatusCodes.Status409Conflict, nameof(UserMessagesRespond.AccountAlreadyExists), UserMessagesRespond.AccountAlreadyExists, UserErrors.EmailAlreadyExist);
        }

        // 1. إنشاء كائن مستخدم جديد (Identity سيقوم بتوليد الـ ID فوراً)
        var user = Request.Adapt<User>();

        // 2. استخدام Mapster لتعبئة البيانات "فوق" الكائن الموجود (Mapping to existing object)
        

        user.UserName = Request.userName.Replace(" ", "_");
        var result = await userManager.CreateAsync(user, Request.Password);

        if (result.Succeeded)
        {
            //Get confirmation Email Token
            var EmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(EmailToken));


            //Send Email
            var emailSent = await SendConfirmEmailAsync(user, code);
            if (!emailSent)
            {
                return Failure(StatusCodes.Status400BadRequest, "Error happened in email service", "we cant send the email to your email right now ", UserErrors.EmailServiceNotWorking);
            }

            return Success(StatusCodes.Status201Created, "User added successfully", " check the email to confirm");
        }

        var Errors = result.Errors.Select(e => new Error(e.Code, e.Description)).ToArray();

        return Failure(StatusCodes.Status400BadRequest, Errors);
    }

    public async Task<resultBase> ConfirmEmail(ConfirmEmailRequest request)
    {
        //check UserID
        if (await userManager.FindByIdAsync(request.UserId) is not { } user)
            return Failure(StatusCodes.Status400BadRequest, UserErrors.InvalidEmailToken);

        //Check if email confirmed
        if (user.EmailConfirmed)
            return Failure(StatusCodes.Status409Conflict, UserErrors.EmailIsAlreadyConfirmed);

        //validate Email Token
        var code = request.Code;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
        }
        catch (Exception)
        {
            return Failure(StatusCodes.Status400BadRequest, UserErrors.InvalidEmailToken);
        }

        var result = await userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, RolesStrings.Member!);
            return Success(StatusCodes.Status200OK, "Email is confirmed successfully", "Email is confirmed successfully now you can login");
        }

        //check if there any Error
        var errors = result.Errors.Select(e => new Error(e.Code, e.Description)).ToArray();
        return Failure(StatusCodes.Status400BadRequest, errors);
    }

    public async Task<resultBase> ReSendConfirmationEmailAsync(ReSendConfirmationEmailRequest request)
    {
        //check email
        if (await userManager.FindByEmailAsync(request.Email) is not { } user)
            return Failure(StatusCodes.Status400BadRequest, UserErrors.UserNotFound);

        //check confirm 
        if (user.EmailConfirmed)
        {
            return Failure(StatusCodes.Status409Conflict, UserErrors.EmailIsAlreadyConfirmed);
        }

        //generate EmailToken
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        //Send the token in email
        var emailSent = await SendConfirmEmailAsync(user, code);
        if (!emailSent)
        {
            return Failure(StatusCodes.Status400BadRequest, "Error happened in email service", "we cant send the email to your email right now ", UserErrors.EmailServiceNotWorking);
        }

        //send id + EmailToken

        return Success(StatusCodes.Status200OK, "the email confirmation has been sent successfully", "the email confirmation has been sent successfully check you mail box");
    }

    public async Task<bool> SendConfirmEmailAsync(User user, string code)
    {


        // build Body
        var body = emailHelper.GetEmailTemplate(EmailTemplatesName.ConfirmEmail, keyValuePairs: new Dictionary<string, string>
        {
            {"name",user.UserName! },
            {"ConfirmLink",$"{clientSettings.ClientUrl}Auth/Confirm?UserId={user.Id}&Code={code}" }
        });

        if (string.IsNullOrEmpty(body))
        {
            return false;
        }

        var sender = await emailHelper.SendEmailAsync(user.Email!, "Email Confirmation", body);

        //send email
        return sender;
    }

    [Obsolete("this method is moved to TokensServices For Better Connectability", true, DiagnosticId = nameof(tokensService))]
    private async Task<(List<string> Roles, List<string> Permissions)> GetUserPermissions(User user)
    {

        var resultRaw = await (
            from ur in db.UserRoles
            where ur.UserId == user.Id      // Get roles IDs for the specific user

            join r in db.Roles on ur.RoleId equals r.Id     // Get roles details

            join p in db.RoleClaims on r.Id equals p.RoleId   // Get permission for each role
            where p.ClaimType == PermissionsDefault.Type // Filter only permission claims

            select new
            {
                RoleName = r.Name,
                Permission = p.ClaimValue
            }
            )
            //.Distinct()   // Im so Confident that this is not needed here
            .ToListAsync();

        (List<string> Roles, List<string> Permissions) =
            (resultRaw.Select(x => x.RoleName!).Distinct().ToList(),
            resultRaw.Select(x => x.Permission!).Distinct().ToList());
        return (Roles, Permissions);
    }
}