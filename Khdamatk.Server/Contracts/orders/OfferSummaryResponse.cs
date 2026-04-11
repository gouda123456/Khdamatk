namespace Khdamatk.Server.Contracts.orders;

public record OfferSummariesResponse(
    List<OneOfferSummaryResponse> Offers
    );
public record OneOfferSummaryResponse(
    OfferServiceDetailed Offer,
    ProviderOfferInfo Provider
    );


