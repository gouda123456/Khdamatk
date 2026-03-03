namespace Khdamatk.Server.Contracts.Jobs;

public record AddJopRequest(
    string UserId,
    string Title,
    string CategoryName,
    string Description,
    List<string> Skills,
    decimal BudgetMin,    
    decimal BudgetMax,
    TimeCommitment TimeCommitment, //TODO: Add TimeCommitment to job Entity
    ExperienceLevel ExperienceLevel,
    DateTime Deadline
    );
public enum TimeCommitment
{
    PartTime,
    FullTime,
    Hourly,
    Flexible
}