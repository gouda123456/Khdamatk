namespace Khdamatk.Server.Contracts.Authentications;

public record RegisterRequest(string userName, string Email, string Password,string? PhoneNumber,bool? IsServiceProvider,string? JobTitle,string? Bio);

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {

        RuleFor(r => r.userName)
            .NotEmpty()
            .Must(r => r.Trim() == r)
            .WithMessage("User Name cant begin or end with white space");

        RuleFor(r => r.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(r => r.Password)
            .SetValidator(new PasswordValidator());

        RuleFor(p => p.PhoneNumber)
            .Matches(@"^[0-9]*$")
            .WithMessage("إذا قمت بإدخال رقم الهاتف، فيجب أن يتكون من أرقام فقط.");

        RuleFor(p => p.IsServiceProvider)
            .NotNull()
            .WithMessage("IsServiceProvider field is required.");


    }

public record RegisterRequest(
    string userName,
    string Email,
    string Password,
    string? PhoneNumber,
    bool? IsServiceProvider,
    string? JobTitle,
    string? Bio
);

}