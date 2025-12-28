namespace Khdamatk.Server.Contracts.Authentications;

public record RegisterRequest(string userName, string Email, string Password);



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
    }

}