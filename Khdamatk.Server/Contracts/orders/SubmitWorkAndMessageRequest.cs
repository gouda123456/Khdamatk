namespace Khdamatk.Server.Contracts.orders;

public record SubmitWorkAndMessageRequest(
    string Message,
    List<IFormFile>? Attachments
    );

public class SubmitWorkAndMessageRequestValidator : AbstractValidator<SubmitWorkAndMessageRequest>
{
    public SubmitWorkAndMessageRequestValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.")
            .MaximumLength(2000).WithMessage("Message cannot exceed 2000 characters.");
        RuleForEach(x => x.Attachments)
            .Must(file => file.Length <= 300 * 1024 * 1024) // 300 MB limit
            .WithMessage("Each attachment must be less than 300 MB.")
            .When(x => x.Attachments != null);
    }
}