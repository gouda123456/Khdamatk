using Khdamatk.Server.Contracts.Admin.Verification;
using Khdamatk.Server.Services.Interfaces;
using Khdamatk.Server.Data.Entities.Identity;
using Khdamatk.Server.ResultPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Khdamatk.Server.Services.Implementations;

public class AdminVerificationService(Database db) : IAdminVerificationService
{
    private readonly Database _db = db;



    public async Task<resultBase> GetPendingVerificationsAsync(string? statusFilter = null, CancellationToken cancellationToken = default)
    {
        // 1. بنعمل Query بتجيب بيانات التوثيق وتعمل Include لبيانات الـ User اللي مربوطة بيها
        var query = _db.Set<VerificationData>()
            .Include(v => v.User)
            .AsQueryable();

        // 2. الفلترة حسب الحالة لو الأدمن اختار حالة معينة من الـ Dropdown فوق
        if (!string.IsNullOrEmpty(statusFilter) && Enum.TryParse<VerificationStatus>(statusFilter, true, out var parsedStatus))
        {
            query = query.Where(v => v.Status == parsedStatus);
        }

        // 3. عمل الـ Mapping للداتا عشان ترجع للجدول زي التصميم بالظبط
        var resultList = await query.Select(v => new AdminVerificationResponse
        {
            UserId = v.UserId,
            FullName = v.User.FullName,
            Email = v.User.Email ?? string.Empty,
            UserRole = v.User.Role ?? "User",
            NationalNumber = v.NationalNumber,
            Country = v.Country,
            City = v.City,
            IdFrontUrl = v.IdFrontUrl,       // كدة هيقراها بسلام بعد ما ضفناها
            IdBackUrl = v.IdBackUrl,         // كدة هيقراها بسلام بعد ما ضفناها
            SelfieWithIdUrl = v.SelfieWithIdUrl, // كدة هيقراها بسلام بعد ما ضفناها
            Status = v.Status.ToString()
        })
        .ToListAsync(cancellationToken);

        return Success(StatusCodes.Status200OK, resultList);
    }

    public async Task<resultBase> ReviewVerificationAsync(ReviewVerificationRequest request, CancellationToken cancellationToken = default)
    {
        // 1. بندور على طلب التوثيق الخاص بالـ User
        var verification = await _db.Set<VerificationData>()
            .FirstOrDefaultAsync(v => v.UserId == request.UserId, cancellationToken);

        if (verification == null)
        {
            return Failure(StatusCodes.Status404NotFound, new Error("Not Found", "طلب التوثيق هذا غير موجود أو تم حذفه."));
        }

        // 2. تحديث الحالة بناءً على قرار الأدمن (Approve / Reject)
        if (request.IsApproved)
        {
            verification.Status = VerificationStatus.Approved;
            // هنا ممكن تزود لوجيك يخلي الـ User الحساب بتاعه Verified بشكل عام في السيستم لو تحب
        }
        else
        {
            verification.Status = VerificationStatus.Rejected;
            // بنسيف الملاحظات في حقل الـ RejectNotes بالداتابيز
            // verification.RejectNotes = request.Notes; 
        }

        _db.Set<VerificationData>().Update(verification);
        await _db.SaveChangesAsync(cancellationToken);

        var actionText = request.IsApproved ? "قبول" : "رفض";
        return Success(StatusCodes.Status200OK, $"تم {actionText} طلب التوثيق بنجاح.");
    }
}