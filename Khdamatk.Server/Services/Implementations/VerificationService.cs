using Khdamatk.Server.Contracts.Verification;
using Khdamatk.Server.Services.Interfaces;
using Khdamatk.Server.Data.Entities.Identity; // الـ Namespace المظبوط للـ VerificationData والـ Enum
using Khdamatk.Server.ResultPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Khdamatk.Server.Services.Implementations;

public class VerificationService : IVerificationService
{
    private readonly Database _db;

    public VerificationService(Database db)
    {
        _db = db;
    }

    public async Task<resultBase> SubmitVerificationAsync(SubmitVerificationRequest request, string userId, CancellationToken cancellationToken = default)
    {
        // ==================== [ خطوة الحماية الجديدة ] ====================
        // نتأكد أولاً إن الـ userId ده له يوزر حقيقي وموجود في قاعدة البيانات
        var userExists = await _db.Users.AnyAsync(u => u.Id == userId, cancellationToken);
        if (!userExists)
        {
            return Failure(StatusCodes.Status404NotFound, new Error("Not Found", "المستخدم غير موجود بالمنظومة أو الـ UserId مبعوت بشكل خاطئ."));
        }
        // ====================================================================

        // 1. جلب طلب التوثيق القديم للمستخدم الحالي
        var existingVerification = await _db.Set<VerificationData>()
            .FirstOrDefaultAsync(v => v.UserId == userId, cancellationToken);

        // التشيك لو الطلب مقبول أو قيد المراجعة لمنع التكرار
        if (existingVerification != null && (existingVerification.Status == VerificationStatus.Pending || existingVerification.Status == VerificationStatus.Approved))
        {
            return Failure(StatusCodes.Status400BadRequest, new Error("Bad Request", "لديك طلب توثيق مسبق بالفعل قيد المراجعة أو مقبول."));
        }

        // 2. رفع الـ 3 صور باستخدام الـ Extension Method
        var frontMedia = await request.IdFront.UploadFileAsync();
        var backMedia = await request.IdBack.UploadFileAsync();
        var selfieMedia = await request.SelfieWithId.UploadFileAsync();

        // 3. إنشاء أو تحديث البيانات
        if (existingVerification == null)
        {
            var newVerification = new VerificationData
            {
                UserId = userId,
                NationalNumber = request.NationalNumber,
                Country = request.Country,
                City = request.City,
                Status = VerificationStatus.Pending
                // ملحوظة: لو الجدول ده فيه Foreign Key تاني لجدول الـ ServiceProviderProfile تأكد من كتابته هنا
            };

            await _db.Set<VerificationData>().AddAsync(newVerification, cancellationToken);
        }
        else
        {
            existingVerification.NationalNumber = request.NationalNumber;
            existingVerification.Country = request.Country;
            existingVerification.City = request.City;
            existingVerification.Status = VerificationStatus.Pending;

            _db.Set<VerificationData>().Update(existingVerification);
        }

        await _db.SaveChangesAsync(cancellationToken);

        return Success(StatusCodes.Status200OK, "تم إرسال طلب التوثيق بنجاح، وهو قيد المراجعة الآن.");
    }

    public async Task<resultBase> GetVerificationStatusAsync(string userId, CancellationToken cancellationToken = default)
    {
        var verification = await _db.Set<VerificationData>()
            .FirstOrDefaultAsync(v => v.UserId == userId, cancellationToken);

        if (verification == null)
        {
            return Success(StatusCodes.Status200OK, "None");
        }

        return Success(StatusCodes.Status200OK, verification.Status.ToString());
    }
}