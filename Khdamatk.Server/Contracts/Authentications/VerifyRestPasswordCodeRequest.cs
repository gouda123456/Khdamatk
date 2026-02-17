namespace Khdamatk.Server.Contracts.Authentications;

public record VerifyRestPasswordCodeRequest(string email,string password, VerificationCodeType CodeType, int Value);

public class VerifyCodeRequestValidator : AbstractValidator<VerifyRestPasswordCodeRequest>
{
    public VerifyCodeRequestValidator()
    {
        RuleFor(x => x.email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
        RuleFor(x => x.password)
            .SetValidator(new PasswordValidator());
        RuleFor(x => x.CodeType)
            .IsInEnum().WithMessage("Invalid code type.");
        RuleFor(x => x.Value)
            .InclusiveBetween(VerificationsCodesConstrains.MinValue, VerificationsCodesConstrains.MaxValue).WithMessage("Code must be a 6-digit number.");

    }
}
