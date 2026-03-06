using Khdamatk.Server.Contracts.Fawaterak;

namespace Khdamatk.Server.Contracts.orders;

public record JobOrderResponse(
    int OrderId,
    OrderType OrderType,
    decimal OfferAmount,
    UserOrderModel Customer,
    UserOrderModel Provider,
    JobSummary JobSummary,
    List<OrderChat> Chat,
    DeliverableFiles DeliverableFiles,
    JobMileStone MileStones
    );

public record UserOrderModel(
    string Id,
    string Name,
    string Email,
    byte[] Picture
    );

public record OrderChat(
    int Id,
    string FromUserId,
    string Content,
    DateTime SendAt

    );

public record DeliverableFiles(
    int Id,
    string FileName,
    long Size,
    DeliveredFileStatues FileStatues
    );

public record JobMileStone(
    int Id,
    string Name,
    string Description,
    bool IsCompleted,
    decimal Price
    );