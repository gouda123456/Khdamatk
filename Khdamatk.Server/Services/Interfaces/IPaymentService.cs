

namespace Khdamatk.Server.Services.Interfaces;

public interface IPaymentService : IService
{
    Task<resultBase> PayToWallet(PayToWalletRequest request, string userId);
    Task<resultBase> Witherdraw(PayToWalletRequest request, string userId);
    Task<resultBase> GetAllTranactions(string userId);
}
