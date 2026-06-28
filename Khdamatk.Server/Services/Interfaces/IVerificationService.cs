using Khdamatk.Server.Contracts.Verification;

namespace Khdamatk.Server.Services.Interfaces;

public interface IVerificationService : IService
{
    // ميثود رفع طلب التوثيق
    Task<resultBase> SubmitVerificationAsync(SubmitVerificationRequest request, string userId, CancellationToken cancellationToken = default);

    Task<resultBase> GetVerificationStatusAsync(string userId, CancellationToken cancellationToken = default);
}
