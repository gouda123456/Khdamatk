namespace Khdamatk.Server.Contracts.Jobs;

public record AddJopOfferRequest(
    string ProviderServiceId, 
    decimal OfferAmount,
    string Description,
    TimeCommitment? TimeCommitment, //TODO: Add TimeCommitment to job Entity
    ExperienceLevel? ExperienceLevel,
    DateTime Deadline,
    byte[]? Attachment,
    string? SimilarWorkExamples 
    );

public class AddJopOfferValidator : AbstractValidator<AddJopOfferRequest>
{
    public AddJopOfferValidator()
    {
        RuleFor(x => x.ProviderServiceId).NotEmpty();
        RuleFor(x => x.OfferAmount).GreaterThan(0);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.Deadline).GreaterThan(DateTime.UtcNow);
    }
}