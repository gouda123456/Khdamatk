namespace Khdamatk.Server.Contracts.Jobs;

public record OfferForServiceResponse(
    ProviderOfferInfo ProviderOfferInfo,
    int OfferId,
    decimal OfferPrice,
    string Description,
    DateTime DeliverAt
    );

