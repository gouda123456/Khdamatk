namespace Khdamatk.Server.Contracts.Home
{

    public record EducationItem(
        string InstitutionName,
        string Specialty,
        string Description,
        string DateRange 
    );

    public record CertificationItem(
        string Name,
        string Description,
        string Date
    );

    public record ExperienceItem(
        string JobTitle,
        string Description
    );

    public record _PortfolioItem(
        string ProjectName,
        string? ImageUrl,
        List<string> Tags
    );
    public record FreelancerProfileResponse(
    string UserId,
    string FullName,
    string JobTitle,
    string Location,
    string MemberSince, 
    double Rating,
    int YearsOfExperience,
    string WorkingHours, 
    string Bio, 
    double HourlyRate,
    List<string> Skills,
    List<_PortfolioItem> Portfolio,
    List<EducationItem> Education,
    List<CertificationItem> Certifications,
    List<ExperienceItem> Experiences,
    string? ProfilePictureUrl,
    string? CoverPictureUrl
);
}
