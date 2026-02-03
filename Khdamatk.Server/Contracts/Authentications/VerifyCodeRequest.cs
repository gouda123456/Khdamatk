namespace Khdamatk.Server.Contracts.Authentications;

public record VerifyCodeRequest(string email, VerificationCodeType CodeType, int Value);

public class VerifyCodeRequestValidator : AbstractValidator<VerifyCodeRequest>
{
    public VerifyCodeRequestValidator()
    {
        RuleFor(x => x.email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
        RuleFor(x => x.CodeType)
            .IsInEnum().WithMessage("Invalid code type.");
        RuleFor(x => x.Value)
            .InclusiveBetween(VerificationsCodesConstrains.MinValue, VerificationsCodesConstrains.MaxValue).WithMessage("Code must be a 6-digit number.");
    }
}
