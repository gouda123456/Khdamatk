using Microsoft.EntityFrameworkCore;
using Khdamatk.Server.Contracts.Admin.Request;
using Khdamatk.Server.Data;
using Khdamatk.Server.ResultPattern;
using Khdamatk.Server.Services.Interfaces;
using Khdamatk.Server.Data.Entities.Operations;
using Microsoft.AspNetCore.Http;

namespace Khdamatk.Server.Services.Implementations;

public class RequestManagementDashboardService(Database db) : IRequestManagementDashboardSerivce
{
    private readonly Database _db = db;

    // 1. جلب قائمة الطلبات الحقيقية بالكامل من الداتابيز داتا لايف
    public async Task<resultBase> GetOrdersAsync(string? statusFilter = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _db.ServiceOrders
                .Include(o => o.Customer)
                    .ThenInclude(c => c.ProfilePicture)
                .Include(o => o.ServiceProviderProfile)
                    .ThenInclude(sp => sp.User)
                        .ThenInclude(u => u.ProfilePicture)
                .Include(o => o.Service)
                .AsNoTracking();

            // الفلترة بالحالة الحقيقية المتوافقة مع الـ Enum
            if (!string.IsNullOrEmpty(statusFilter) && Enum.TryParse<OrderStatus>(statusFilter, true, out var parsedStatus))
            {
                query = query.Where(o => o.Status == parsedStatus);
            }

            var orders = await query
                .OrderByDescending(o => o.CreatedAt) // الترتيب حسب تاريخ الإنشاء من الـ BaseEntity
                .Select(o => new RequestManagementDashboard
                {
                    OrderId = "B" + o.Id.ToString(),
                    ClientName = o.Customer != null ? (o.Customer.FullName ?? "عميل") : "عميل غير معروف",

                    // 🛡️ طريقة أمان لتفادي خطأ اسم حقل الـ Media: لو موجودة حط قيمة نصية أو الـ Id بتاعها لحين استعراض كلاس الميديا
                    ClientImageUrl = o.Customer != null && o.Customer.ProfilePicture != null
                        ? (o.Customer.ProfilePictureId.ToString() ?? string.Empty)
                        : string.Empty,

                    ProviderName = o.ServiceProviderProfile != null && o.ServiceProviderProfile.User != null
                        ? (o.ServiceProviderProfile.User.FullName ?? "مقدم الخدمة")
                        : "مقدم خدمة غير معروف",

                    ProviderImageUrl = o.ServiceProviderProfile != null && o.ServiceProviderProfile.User != null && o.ServiceProviderProfile.User.ProfilePicture != null
                        ? (o.ServiceProviderProfile.User.ProfilePictureId.ToString() ?? string.Empty)
                        : string.Empty,

                    ServiceName = o.Service != null ? (o.Service.Title ?? "خدمة") : "Graphic Design",

                    Price = o.Amount, // حقل السعر الحقيقي الموروث من الـ OrderBase
                    Status = o.Status.ToString(),
                    Date = o.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Success(StatusCodes.Status200OK, orders);
        }
        catch (Exception ex)
        {
            return Failure(StatusCodes.Status500InternalServerError, new Error("ServerError", ex.Message));
        }
    }

    // 2. حساب كروت الإحصائيات من واقع حالات الجدول الحقيقية
    public async Task<resultBase> GetOrderAnalyticsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var statuses = await _db.ServiceOrders
                .Select(o => o.Status)
                .ToListAsync(cancellationToken);

            var analytics = new OrderAnalyticsResponse
            {
                TotalRequests = statuses.Count,
                PendingCount = statuses.Count(s => s == OrderStatus.Pending),
                CompletedCount = statuses.Count(s => s == OrderStatus.Completed),
                CancelledCount = statuses.Count(s => s == OrderStatus.Canceled)
            };

            return Success(StatusCodes.Status200OK, analytics);
        }
        catch (Exception ex)
        {
            return Failure(StatusCodes.Status500InternalServerError, new Error("ServerError", ex.Message));
        }
    }

    // 3. تحديث حالة الطلب الفعلي وحفظه
    public async Task<resultBase> UpdateOrderAsync(UpdateOrderAdminRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _db.ServiceOrders
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null)
            {
                return Failure(StatusCodes.Status404NotFound, new Error("NotFound", "الطلب المطلوب غير موجود."));
            }

            if (Enum.TryParse<OrderStatus>(request.Status, true, out var newStatus))
            {
                order.Status = newStatus;
            }
            else
            {
                return Failure(StatusCodes.Status400BadRequest, new Error("ValidationError", "حالة الطلب المرسلة غير صالحة."));
            }

            if (request.Attachment != null)
            {
                var media = await request.Attachment.UploadFileAsync();
                order.MediaAttachments.Add(media);
            }

            await _db.SaveChangesAsync(cancellationToken);

            return Success(StatusCodes.Status200OK, "تم تحديث حالة الطلب في الداتابيز بنجاح.");
        }
        catch (Exception ex)
        {
            return Failure(StatusCodes.Status500InternalServerError, new Error("ServerError", ex.Message));
        }
    }
}