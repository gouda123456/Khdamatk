namespace Khdamatk.Server.Contracts.Jobs;

public record OfferDetailedForServiceResponse(
    ProviderOfferDetailedInfo ProviderOfferInfo,
    OfferServiceDetailed OfferServiceDetailed,
    JobSummary JobSummary
    );

public record ProviderOfferInfo(
    string ProviderId,
    int OfferId,
    string ProviderName,
    string ProviderJobTitle,
    double ProviderRate,
    byte[] ProviderProfile
    );

public record OfferServiceDetailed(
    int Id,
    decimal Amount,
    int DeliversInDays,
    string Description
    );
public record ProviderOfferDetailedInfo(
    string Id,
    string Name,
    string Address,
    string JobTitle,
    double ProviderRate,
    int ExperienceInYears,
    bool IsVerified,
    byte[] ProviderProfile
    );

public record JobSummary(
    int Id,
    string Title,
    decimal BudgetMin,
    decimal BudgetMax,
    int DeliversInDays,
    DateTime Deadline,
    ExperienceLevel ExperienceLevel,
    List<string> Skills,
    string Description,
    List<MileStone> MileStones
    );