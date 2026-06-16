using Azure;

namespace Khdamatk.Server.Services.Implementations;

public class PaymentService(Database db) : IPaymentService
{
    private readonly Database db = db;

    public async Task<resultBase> GetAllTranactions(string userId)
    {

        var totalBalance = await db.JobOrders
            .SumAsync(jb => jb.PaymentTransaction.Amount) + await db.ServiceOrders.SumAsync(sb =>sb.PaymentTransaction.Amount);


        var AvalibleBalance = await db.JobOrders
            .Where(o => o.Status == OrderStatus.Completed || o.Status == OrderStatus.CancelledByProvider || o.Status == OrderStatus.CancelledByClient)
            .SumAsync(jb => jb.PaymentTransaction.Amount) + await db.ServiceOrders.SumAsync(sb => sb.PaymentTransaction.Amount);

        var WitherdrawBalance = db.Users.Where(u => u.Id == userId).FirstOrDefault()?.Amount ?? 0;

        var Balance = new BalanceDetailResponse(
                    TotalBalance: totalBalance ,
                    AvalibleBalance: AvalibleBalance,
                    PendingBalance: totalBalance - AvalibleBalance,
                    WitherdrawBalance: WitherdrawBalance
                    );

        var JobOrderTransactions = await db.JobOrders
            .Where(o => o.ServiceProviderId == userId || o.CustomerId == userId)
            .Select(jb => new TransactionDetailResponse(
                jb.PaymentTransaction.Id,
                orderType.JobOrder,
                jb.Id,
                jb.Job.Title,
                jb.PaymentTransaction.Amount,
                jb.PaymentTransaction.Status,
                jb.PaymentTransaction.TransactionDate
            ))
            .ToListAsync();

        var ServiceOrderTransactions = await db.ServiceOrders
            .Where(o => o.ServiceProviderId == userId || o.CustomerId == userId)
            .Select(so => new TransactionDetailResponse(
                so.PaymentTransaction.Id,
                orderType.ServiceOrder,
                so.Id,
                so.Service.Title,
                so.PaymentTransaction.Amount,
                so.PaymentTransaction.Status,
                so.PaymentTransaction.TransactionDate
            ))
            .ToListAsync();

        var Transactions = JobOrderTransactions.Concat(ServiceOrderTransactions).OrderByDescending(t => t.Date).ToList();

        var response = new PaymrntPageDetailResponse(
            Balance: Balance,
            RecentTransactions: Transactions
        );


        return Success(StatusCodes.Status200OK, "Payment details retrieved successfully", "Payment details retrieved successfully", response);
    }
    

    public async Task<resultBase> PayToWallet(PayToWalletRequest request, string userId)
    {
        return Success(StatusCodes.Status200OK,SuccessMessages.General.Title,SuccessMessages.General.Message);
    }

    public async Task<resultBase> Witherdraw(PayToWalletRequest request, string userId)
    {
        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message);
    }
}
