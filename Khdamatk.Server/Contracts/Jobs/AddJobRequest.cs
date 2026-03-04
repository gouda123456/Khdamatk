namespace Khdamatk.Server.Contracts.Jobs;

public record AddJobRequest(
    string UserId,
    string Title,
    string CategoryName,
    string Description,
    List<string> Skills,
    decimal BudgetMin,    
    decimal BudgetMax,
    TimeCommit TimeCommitment, //TODO: Add TimeCommitment to job Entity
    ExperienceLevel ExperienceLevel,
    ICollection<Media>? Media,
    DateTime Deadline
    );
public enum TimeCommit
{
    PartTime,
    FullTime,
    Hourly,
    Flexible
}

