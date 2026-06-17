namespace Khdamatk.Server.Contracts.Payment;

public record PaymrntPageDetailResponse(
    BalanceDetailResponse Balance,
    List<TransactionDetailResponse> RecentTransactions
    );

public record BalanceDetailResponse(
    decimal TotalBalance,
    decimal AvalibleBalance,
    decimal PendingBalance,
    decimal WitherdrawBalance
    );

public record TransactionDetailResponse(
    int Id,
    orderType Type,
    int orderId,
    string orderName,
    decimal Amount,
    TransactionStatus Status,
    DateTime Date
    );

public enum orderType
{
    ServiceOrder,
    JobOrder,
    WalletTransaction
}
