namespace Khdamatk.Server.Contracts.Authentications;


public record LoginRequest(string Email, string Password);


public record LoginResponse(JwtToken JwtToken, RefreshToken RefreshToken);



public class UserValidation : AbstractValidator<LoginRequest>
{
    public UserValidation()
    {


        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.Password).SetValidator(new PasswordValidator());
    }
}


public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(p => p)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
            .MaximumLength(100).WithMessage("Password must be at most 100 characters long")
            .Matches(UserRegex.PasswordRegex)
            .WithMessage("Password must contain uppercase, lowercase, digit, and special character, and no spaces or triple repeated characters.");
    }
}