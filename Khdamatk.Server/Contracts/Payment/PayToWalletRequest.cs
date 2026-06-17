namespace Khdamatk.Server.Contracts.Payment;

public record PayToWalletRequest(
    decimal Amount,
    string UserId
    );

public class PayToWalletRequestValidator : AbstractValidator<PayToWalletRequest>
{
    public PayToWalletRequestValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}
