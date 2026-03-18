namespace Khdamatk.Server.Contracts.Service;

public record ServiceDetailsResponse(
    int ServiceId,
    string ServiceTitle,
    string ShortDescription,
    string DetailDescription,
    decimal Price,
    Byte[] MainImage,
    List<Byte[]> ServiceImages,
    ProviderServiceInfo ProviderServiceInfo
    );

public record ProviderServiceInfo(
    string Id,
    string Name,
    string JobTitle,
    byte[] Image,
    int AverageRating,
    int AverageResponseTime,
    int TotalOrdersInProgress,
    int NumberOfRequests,
    int TotalOrdersCompleted,
    int DelivererTimeInDays
    );


public class ServiceDetailsRequestValidator : AbstractValidator<ServiceDetailsResponse>
{
    public ServiceDetailsRequestValidator()
    {
        RuleFor(x => x.ServiceId).GreaterThan(0);
        RuleFor(x => x.ServiceTitle).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ShortDescription).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DetailDescription).NotEmpty();
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MainImage).NotNull();
        RuleFor(x => x.ServiceImages).NotNull();
        RuleForEach(x => x.ServiceImages).NotNull();
        RuleFor(x => x.ProviderServiceInfo).NotNull();
        RuleFor(x => x.ProviderServiceInfo.Id).NotEmpty();
        RuleFor(x => x.ProviderServiceInfo.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ProviderServiceInfo.JobTitle).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ProviderServiceInfo.Image).NotNull();
        RuleFor(x => x.ProviderServiceInfo.AverageRating).InclusiveBetween(0, 5);
        RuleFor(x => x.ProviderServiceInfo.AverageResponseTime).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ProviderServiceInfo.TotalOrdersInProgress).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ProviderServiceInfo.NumberOfRequests).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ProviderServiceInfo.TotalOrdersCompleted).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ProviderServiceInfo.DelivererTimeInDays).GreaterThanOrEqualTo(0);
    }
}