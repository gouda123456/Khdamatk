namespace Khdamatk.Server.Contracts.Home;

public record UpdateProfileRequest(
    string JobTitle,
    string? Bio,
    double HourlyRate,
    int ExperienceYears
);
