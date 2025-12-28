namespace Khdamatk.Server.Contracts.Authentications;

public record ReSendConfirmationEmailRequest(string Email);

public class ReSendConfirmationEmailRequestValidator : AbstractValidator<ReSendConfirmationEmailRequest>
{
    public ReSendConfirmationEmailRequestValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty()
            .EmailAddress();
    }
}