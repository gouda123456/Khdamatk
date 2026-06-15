using Khdamatk.Server.Contracts.Fawaterak;

namespace Khdamatk.Server.Contracts.orders;

public record ServiceOrderResponse(
    int OrderId,
    OrderType OrderType,
    OrderStatus Status,
    decimal FinalPrice,
    UserOrderModel Customer,
    UserOrderModel Provider,
    ServiceSummary ServiceSummary,
    List<OrderChat> Chat,
    List<DeliverableFiles> DeliverableFiles,
    DateTime CreatedAt,
    DateTime? CompletedAt
);

public record ServiceSummary(
    int Id,
    string Title,
    decimal Price,
    int DeliveryTimeInDays,
    int RevisionCount,
    string Description
);
