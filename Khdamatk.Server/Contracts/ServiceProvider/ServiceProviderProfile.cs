namespace Khdamatk.Server.Contracts.Home;


public record AddPortfolioRequest(
    string Title,
    string Description,
    string ImageUrl
);

public class AddPortfolioRequestValidator : AbstractValidator<AddPortfolioRequest>
{
    public AddPortfolioRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Image URL is required.")
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute)).WithMessage("Invalid URL format.");
    }
}



public record AddEducationRequest(
    string SchoolName,
    string Degree,
    string FieldOfStudy,
    string Description,
    DateTime StartDate,
    DateTime? EndDate
);


public record AddExperienceRequest(
    string Title,
    string CompanyName,
    string Description,
    DateTime StartDate,
    DateTime? EndDate
);

public record UpdateSkillsRequest(
    List<int> SkillIds
);
public record AddCertificateRequest(
    string Title,
    string Issuer,
    string Type,
    int YearAcquired
);

public record UpdateProfileRequest(
    string JobTitle,
    string Bio,
    double HourlyRate,
    int ExperienceYears,
    string? FacebookUrl,
    string? LinkedInUrl,
    string? GithubUrl,
    string? TwitterUrl
);

public record UpdateEducationRequest(
    int Id,
    string SchoolName,
    string Degree,
    string FieldOfStudy,
    string Description,
    DateTime StartDate,
    DateTime EndDate
    );
public record UpdateExperienceRequest(
    int Id,
    string Title,
    string CompanyName,
    string Description,
    DateTime StartDate,
    DateTime EndDate
    );