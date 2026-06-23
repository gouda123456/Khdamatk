using Microsoft.EntityFrameworkCore;
using Khdamatk.Server.Data;
using Khdamatk.Server.ResultPattern;
using Khdamatk.Server.Services.Interfaces;
using Khdamatk.Server.Contracts.Admin.Disputes;
using Khdamatk.Server.Data.Entities.Interaction; // تأكد من عمل using للمجلد اللي فيه الـ Dispute
using Microsoft.AspNetCore.Http;

namespace Khdamatk.Server.Services.Implementations;

public class FinalDecisionService(Database db) : IFinalDecisionService
{
    private readonly Database _db = db;

    // 1. جلب البيانات الحقيقية من الداتابيز متوافقة مع الـ Entity بتاعتك
    public async Task<resultBase> GetDecisionDetailsAsync(int disputeId, CancellationToken cancellationToken = default)
    {
        try
        {
            // بنجيب النزاع وبنعمل Include للعلاقات الحقيقية (Raiser و Target و AdminReviewer)
            var dispute = await _db.Disputes
                .Include(d => d.Raiser)          // الطرف الرافع للنزاع (Client أو Provider)
                .Include(d => d.Target)          // الطرف المدعى عليه (Client أو Provider)
                .Include(d => d.AdminReviewer)    // المسؤول المعين لإدارة النزاع
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == disputeId, cancellationToken);

            if (dispute == null)
            {
                return Failure(StatusCodes.Status404NotFound, new Error("NotFound", "هذا النزاع غير موجود في الداتابيز."));
            }

            // ربط الحقول الحقيقية بالـ DTO Response الخاص بالشاشة
            var decision = new FinalDecisionResponse
            {
                ReportId = $"RPT-2026-{dispute.Id:D6}", // توليد رقم تقرير مميز بناءً على الـ Id
                Type = dispute.Type.ToString(),         // نوع النزاع من الـ Enum (مثل QualityIssue)
                ClientName = dispute.Raiser?.FullName ?? "الطرف الرافع",
                FreelancerName = dispute.Target?.FullName ?? "الطرف المدعى عليه",
                ResolvedBy = dispute.AdminReviewer?.FullName ?? "لم يتم التعيين بعد",
                SubmittedDate = dispute.OpenedDate,
                ReviewedDate = dispute.ResolutionDate,
                ResolutionTime = dispute.ResolutionDate != null
                    ? $"{(dispute.ResolutionDate.Value - dispute.OpenedDate).Days} days {(dispute.ResolutionDate.Value - dispute.OpenedDate).Hours} hours"
                    : "Under Review",

                // تحويل الـ Enum لـ string عشان يتعرض في الشاشة
                ClaimStatus = dispute.Status.ToString().ToUpper(),
                CompensationAmount = dispute.AmountUnderDispute ?? 0m, // المبلغ المتنازع عليه من الداتا
                CompensationType = "Refund Decision",
                DecisionNotes = dispute.FinalDecisionDetails ?? dispute.ReasonDetails ?? "لا توجد ملاحظات مسجلة للقرار حالياً."
            };

            return Success(StatusCodes.Status200OK, decision);
        }
        catch (Exception ex)
        {
            return Failure(StatusCodes.Status500InternalServerError, new Error("ServerError", ex.Message));
        }
    }

    // 2. استقبال وحفظ القرار الفعلي للأدمن وتعديل حالة الـ Enum في الداتابيز
    public async Task<resultBase> SubmitDecisionAsync(SubmitDecisionRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            // جلب السجل الحقيقي للتعديل عليه
            var dispute = await _db.Disputes.FirstOrDefaultAsync(d => d.Id == request.DisputeId, cancellationToken);

            if (dispute == null)
            {
                return Failure(StatusCodes.Status404NotFound, new Error("NotFound", "النزاع المراد اتخاذ قرار بشأنه غير موجود."));
            }

            // تحديث البيانات بناءً على قرار الأدمن والحقول الحقيقية للـ Entity
            // بنغير الحالة لـ AwaitingConfirmation (بانتظار موافقة الأطراف) أو Resolved مباشرة حسب بيزنس مشروعكم
            dispute.Status = request.IsApproved ? DisputeStatus.AwaitingConfirmation : DisputeStatus.UnderReview;
            dispute.FinalDecisionDetails = request.DecisionNotes;
            dispute.ResolutionDate = DateTime.UtcNow;

            // 📸 رفع ومعالجة الصور المرفقة بالقرار (لو الشاشة بتبعت مرفقات)
            if (request.Attachments != null && request.Attachments.Count > 0)
            {
                foreach (var item in request.Attachments)
                {
                    // استدعاء الـ Extension Method بتاعتكم لرفع الصورة فوراً للسيرفر
                    var media = await item.UploadFileAsync();

                    // لو عندكم جدول وسيط لملفات النزاع تقدر تضيفه هنا، لو مش موجود سيبها كدة الميديا هترفع عادي
                    // _db.DisputeEvidences.Add(new DisputeEvidence { DisputeId = dispute.Id, MediaId = media.Id });
                }
            }

            // حفظ التعديلات النهائية في الداتابيز رسمياً 🚀
            await _db.SaveChangesAsync(cancellationToken);

            return Success(StatusCodes.Status200OK, "تم اعتماد القرار النهائي للأدمن وتحديث حالة النزاع في الداتابيز بنجاح.");
        }
        catch (Exception ex)
        {
            return Failure(StatusCodes.Status500InternalServerError, new Error("ServerError", ex.Message));
        }
    }
}