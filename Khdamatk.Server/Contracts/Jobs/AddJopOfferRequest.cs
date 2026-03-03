namespace Khdamatk.Server.Contracts.Jobs;

public record AddJopOfferRequest(
    decimal OfferAmount,
    string CoverLetter
    );
