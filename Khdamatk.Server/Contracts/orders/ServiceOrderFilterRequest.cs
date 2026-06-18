namespace Khdamatk.Server.Contracts.orders;

public record ServiceOrderFilterRequest(
    OrderStatus? Status,
    DateTime? FromDate,
    DateTime? ToDate,
    decimal? MinPrice,
    decimal? MaxPrice,
    int PageNumber = 1,
    int PageSize = 10,
    string? SortBy = "CreatedAt",
    bool SortDescending = true
);
