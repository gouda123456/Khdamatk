using Khdamatk.Server.Contracts.Verification;

namespace Khdamatk.Server.Services.Interfaces;

public interface IVerificationService : IService
{
    // ميثود رفع طلب التوثيق
    Task<resultBase> SubmitVerificationAsync(SubmitVerificationRequest request, string userId, CancellationToken cancellationToken = default);

    // ميثود إضافية لمعرفة حالة التوثيق الحالية للمستخدم (عشان الـ Frontend يعرف يعرض أنهي شاشة)
    Task<resultBase> GetVerificationStatusAsync(string userId, CancellationToken cancellationToken = default);
}
