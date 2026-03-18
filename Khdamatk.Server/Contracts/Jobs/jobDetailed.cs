namespace Khdamatk.Server.Contracts.Jobs;

//TODO: Separate Contract content to multiple records for better maintainability and readability 
public record JobDetailed(
    int Id,
    string Title,
    int OffersCount,
    ExperienceLevel ExperienceLevel,
    string ProjectLength,
    decimal BudgetMin,
    decimal BudgetMax,
    string Description,
    JobPostStatus Status,
    DateTime CreatedAt,
    DateTime Deadline,
    string CustomerId,
    string CustomerName,
    int CategoryId,
    string CategoryName,
    string TimeCommitment,
    IEnumerable<string> ImageUrls,
    IEnumerable<string> RequiredSkills
    );

