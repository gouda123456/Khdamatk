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
); public record JobCard1(
    string Id,
    string JobTitle,
    string JobDescription,
    DateTime PostedDate,
    double BudgetMin,
    double BudgetMax
); public record JobCard2(
    string Id,
    string JobTitle,
    string JobDescription,
    DateTime PostedDate,
    double BudgetMin,
    double BudgetMax
);