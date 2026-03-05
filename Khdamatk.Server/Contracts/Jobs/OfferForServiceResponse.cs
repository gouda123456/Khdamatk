namespace Khdamatk.Server.Contracts.Jobs;

public record OfferForServiceResponse(
    ProviderOfferInfo ProviderOfferInfo,
    decimal OfferPrice,
    string Description
    );

