using Khdamatk.Server.Contracts.Admin.Verification;
using Khdamatk.Server.ResultPattern;

namespace Khdamatk.Server.Services.Interfaces;

public interface IAdminVerificationService : IService
{
    // جلب كل الطلبات مع إمكانية الفلترة حسب الـ Status (اختياري)
    Task<resultBase> GetPendingVerificationsAsync(string? statusFilter = null, CancellationToken cancellationToken = default);

    // اتخاذ قرار بالقبول أو الرفض
    Task<resultBase> ReviewVerificationAsync(ReviewVerificationRequest request, CancellationToken cancellationToken = default);
}