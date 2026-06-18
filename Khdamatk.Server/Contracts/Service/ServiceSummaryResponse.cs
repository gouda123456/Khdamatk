namespace Khdamatk.Server.Contracts.Service;

public record ServiceSummaryResponse(
    int Id,
    string Title,
    string ShortDescription,
    decimal Price,
    byte[] MainImage,
    int OrdersCount,
    double AverageRating,
    int DeliveryTimeInDays,
    ProviderSummaryInfo ProviderInfo
);

public record ProviderSummaryInfo(
    string Id,
    string Name,
    byte[] ProfileImage,
    double Rating
);
