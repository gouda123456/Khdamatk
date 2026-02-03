namespace Khdamatk.Server.Contracts.Home;

public record JobsPage(
    List<string> Services,
    List<JobCard> JobCards
    );

public record JobCard(
    string Id,
    string JobTitle,
    string JobDescription,
    DateTime PostedDate,
    double BudgetMin,
    double BudgetMax
);