namespace Khdamatk.Server.Contracts.Service;

public record ServiceFilterRequest(
    string? SearchTerm,
    string? CategoryName,
    decimal? MinPrice,
    decimal? MaxPrice,
    int? MinDeliveryDays,
    int? MaxDeliveryDays,
    ExperienceLevel? ExperienceLevel,
    double? MinRating,
    int PageNumber = 1,
    int PageSize = 10,
    string? SortBy = "CreatedAt",
    bool SortDescending = true
);
