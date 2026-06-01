namespace Khdamatk.Server.Contracts.Service;

public record ServiceDetailsResponse(
    int ServiceId,
    string ServiceTitle,
    string ShortDescription,
    string DetailDescription,
    decimal Price,
    int RevisionCount,
    int DeliveryTimeInDays,
    ExperienceLevel ExperienceLevel,
    List<string> Concepts,
    byte[] MainImage,
    List<byte[]> ServiceImages,
    int OrdersCount,
    double AverageRating,
    ProviderServiceInfo ProviderServiceInfo
);

public record ProviderServiceInfo(
    string Id,
    string Name,
    string JobTitle,
    byte[] Image,
    double AverageRating,
    int AverageResponseTime,
    int TotalOrdersInProgress,
    int NumberOfRequests,
    int TotalOrdersCompleted
);
