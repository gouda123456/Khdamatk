using Khdamatk.Server.Contracts.Fawaterak;

namespace Khdamatk.Server.Contracts.orders;

public record ServiceOrderSummaryResponse(
    int OrderId,
    OrderType OrderType,
    OrderStatus Status,
    decimal FinalPrice,
    string CustomerName,
    string ProviderName,
    string ServiceTitle,
    DateTime CreatedAt,
    DateTime? Deadline,
    int UnreadMessagesCount
);
