namespace Khdamatk.Server.Contracts.Service;

public record OrderServiceRequest(
    string? AdditionalDetails,
    decimal? Price
    );
