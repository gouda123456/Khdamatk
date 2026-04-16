namespace Khdamatk.Server.Contracts.orders;

public record ReviewRequest(
    string Title,
    string Content,
    double Rating,
    int OrderId
);
