using Khdamatk.Server.Contracts.Admin.Review;
using Khdamatk.Server.ResultPattern;

namespace Khdamatk.Server.Services.Interfaces;

public interface IAdminReviewService : IService
{
    // 1. جلب قائمة التقييمات مع إمكانية الفلترة حسب الحالة (Visible / Flagged)
    Task<resultBase> GetReviewsAsync(string? statusFilter = null, CancellationToken cancellationToken = default);

    // 2. تغيير حالة التقييم (إخفاء أو إظهار)
    Task<resultBase> ModerateReviewAsync(ModerateReviewRequest request, CancellationToken cancellationToken = default);

    // 3. جلب الإحصائيات الخاصة بالـ Analytics لوحة التحكم
    Task<resultBase> GetReviewAnalyticsAsync(CancellationToken cancellationToken = default);
}