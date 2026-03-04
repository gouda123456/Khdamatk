namespace Khdamatk.Server.Contracts.Jobs;

public record OfferForServiceResponse(
    string ProviderId,
    int OfferId,
    string ProviderName,
    string ProviderJobTitle,
    double ProviderRate,
    byte[] ProviderProfile,
    decimal OfferPrice,
    string Description
    );

