namespace Khdamatk.Server.Contracts.Home;

public record JobsPage(
    List<ServiceItem> Services,
    List<JobCard> JobCards
    );

public record ServiceItem (
    int Id,
    string Name
    );

public record JobCard(
    int Id,
    string JobTitle,
    string JobDescription,
    string Category,
    DateTime PostedDate,
    double BudgetMin,
    double BudgetMax
);

public record JobsFilterRequest(
    string? Search,
    int? ServiceId,
    string? ExperienceLevel,
    int Page = 1,
    int PageSize = 10
);