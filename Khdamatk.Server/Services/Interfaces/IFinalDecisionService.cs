using Khdamatk.Server.ResultPattern;
using Khdamatk.Server.Contracts.Admin.Disputes;

namespace Khdamatk.Server.Services.Interfaces;

public interface IFinalDecisionService : IService
{
    // جلب بيانات القرار النهائي لعرضها في الشاشة
    Task<resultBase> GetDecisionDetailsAsync(int disputeId, CancellationToken cancellationToken = default);

    // تنفيذ وحفظ القرار النهائي للأدمن مع معالجة الصور المرفوعة
    Task<resultBase> SubmitDecisionAsync(SubmitDecisionRequest request, CancellationToken cancellationToken = default);
}
