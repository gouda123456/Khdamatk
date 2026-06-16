using Khdamatk.Server.ResultPattern;
using Khdamatk.Server.Contracts.Admin.Request;

namespace Khdamatk.Server.Services.Interfaces;

public interface IRequestManagementDashboardSerivce : IService
{
    // جلب قائمة الطلبات مع فلترة اختيارية بحالة الطلب
    Task<resultBase> GetOrdersAsync(string? statusFilter = null, CancellationToken cancellationToken = default);

    // جلب أرقام وإحصائيات الكروت العلوية
    Task<resultBase> GetOrderAnalyticsAsync(CancellationToken cancellationToken = default);

    // تحديث حالة طلب معين مع إمكانية إرفاق ميديا/صورة
    Task<resultBase> UpdateOrderAsync(UpdateOrderAdminRequest request, CancellationToken cancellationToken = default);
}
