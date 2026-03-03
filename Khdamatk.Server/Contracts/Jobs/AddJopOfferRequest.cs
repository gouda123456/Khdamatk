namespace Khdamatk.Server.Contracts.Jobs;

public record AddJopOfferRequest(
    string ProviderServiceId, 
    decimal OfferAmount,
    string Description,
    TimeCommitment? TimeCommitment, //TODO: Add TimeCommitment to job Entity
    ExperienceLevel? ExperienceLevel,
    DateTime Deadline,
    byte[]? Attachment,
    string? SimilarWorkExamples 
    );
