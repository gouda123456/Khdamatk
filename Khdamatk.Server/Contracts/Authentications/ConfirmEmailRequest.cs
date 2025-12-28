using FluentValidation;

namespace Khdamatk.Server.Contracts.Authentications;

public record ConfirmEmailRequest(string UserId, string Code);

public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailRequestValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty();
        RuleFor(c => c.Code)
            .NotEmpty();
    }
}