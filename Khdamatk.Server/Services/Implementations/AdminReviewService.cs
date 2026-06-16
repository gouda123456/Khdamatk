using Microsoft.EntityFrameworkCore;
using Khdamatk.Server.Contracts.Admin.Review;
using Khdamatk.Server.Data;
using Khdamatk.Server.ResultPattern;
using Khdamatk.Server.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Khdamatk.Server.Services.Implementations;

public class AdminReviewService(Database db) : IAdminReviewService
{
    private readonly Database _db = db;

    public async Task<resultBase> GetReviewsAsync(string? statusFilter = null, CancellationToken cancellationToken = default)
    {
        try
        {
            // سحب التقييمات مع عمل Include للـ Reviewer
            var query = _db.Reviews
                .Include(r => r.Reviewer)
                .AsNoTracking();

            var reviews = await query
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new AdminReviewResponse
                {
                    ReviewId = r.Id,
                    // تأمين اسم المستخدم تماماً من الـ Null جوه الداتابيز
                    ReviewerName = r.Reviewer != null ? (r.Reviewer.FullName ?? "مستخدم غير معروف") : "مستخدم غير معروف",
                    ReviewerImageUrl = string.Empty,
                    Rating = (int)r.Rating,
                    ReviewText = r.Content ?? string.Empty,
                    Status = "Visible",
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Success(StatusCodes.Status200OK, reviews);
        }
        catch (Exception ex)
        {
            // لو ضربت 500، الـ Breakpoint هنا هيمسك الأيرور وهيقولك ماله بالظبط في الـ ex.Message
            return Failure(StatusCodes.Status500InternalServerError, new Error("ServerError", ex.Message));
        }
    }

    public async Task<resultBase> ModerateReviewAsync(ModerateReviewRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var review = await _db.Reviews.FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken);

            if (review == null)
            {
                return Failure(StatusCodes.Status404NotFound, new Error("Not Found", "التقييم المطلوب غير موجود."));
            }

            return Success(StatusCodes.Status200OK, "تم تحديث حالة التقييم بنجاح (وضع افتراضي).");
        }
        catch (Exception ex)
        {
            return Failure(StatusCodes.Status500InternalServerError, new Error("ServerError", ex.Message));
        }
    }

    public async Task<resultBase> GetReviewAnalyticsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var allRatings = await _db.Reviews.Select(r => r.Rating).ToListAsync(cancellationToken);

            if (!allRatings.Any())
                return Success(StatusCodes.Status200OK, new ReviewAnalyticsResponse());

            var analytics = new ReviewAnalyticsResponse
            {
                TotalReviews = allRatings.Count,
                AverageRating = allRatings.Any() ? Math.Round(allRatings.Average(), 1) : 0,
                FiveStarCount = allRatings.Count(r => r >= 5),
                FourStarCount = allRatings.Count(r => r >= 4 && r < 5),
                ThreeStarCount = allRatings.Count(r => r >= 3 && r < 4),
                TwoStarCount = allRatings.Count(r => r >= 2 && r < 3),
                OneStarCount = allRatings.Count(r => r >= 1 && r < 2)
            };

            return Success(StatusCodes.Status200OK, analytics);
        }
        catch (Exception ex)
        {
            return Failure(StatusCodes.Status500InternalServerError, new Error("ServerError", ex.Message));
        }
    }
}