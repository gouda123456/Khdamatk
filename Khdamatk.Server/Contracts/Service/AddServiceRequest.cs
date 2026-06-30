using MailKit;
using Khdamatk.Server.Helper.Validations;

namespace Khdamatk.Server.Contracts.Service;

public record AddServiceRequest(
    string ProviderProfileId,
    string Title,
    string CategoryName,
    string ShortDescription,
    string DetailedDescription,
    decimal Price,
    int RevisionCount,
    List<string> Concepts,
    int DeliverTimeInDays, 
    ExperienceLevel ExperienceLevel,
    IFormFile? ServiceEnvelope,
    IFormFileCollection Attachment
);

public class AddServiceValidator : AbstractValidator<AddServiceRequest>
{
    public AddServiceValidator()
    {
        RuleFor(x => x.ProviderProfileId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CategoryName).NotEmpty();
        RuleFor(x => x.ShortDescription).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DetailedDescription).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.RevisionCount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Concepts).NotEmpty();
        RuleFor(x => x.DeliverTimeInDays).GreaterThan(0);
    }
}