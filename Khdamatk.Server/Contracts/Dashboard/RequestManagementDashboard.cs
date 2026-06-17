using Microsoft.AspNetCore.Http;

namespace Khdamatk.Server.Contracts.Admin.Request;

// 1. يمثل البيانات التي ستعرض داخل جدول الطلبات
public class RequestManagementDashboard
{
    public string OrderId { get; set; } = string.Empty; // مثل: B230038
    public string ClientName { get; set; } = string.Empty; // اسم العميل
    public string ClientImageUrl { get; set; } = string.Empty; // رابط صورة العميل إن وجد
    public string ProviderName { get; set; } = string.Empty; // اسم مقدم الخدمة
    public string ProviderImageUrl { get; set; } = string.Empty; // رابط صورة مقدم الخدمة
    public string ServiceName { get; set; } = string.Empty; // اسم الخدمة (مثال: Graphic Design)
    public decimal Price { get; set; } // السعر
    public string Status { get; set; } = string.Empty; // حالة الطلب (Paid, Pending, Completed, Cancelled)
    public DateTime Date { get; set; } // تاريخ الطلب
}

// 2. يمثل الإحصائيات الخاصة بالكروت الأربعة العلوية
public class OrderAnalyticsResponse
{
    public int TotalRequests { get; set; } // إجمالي الطلبات
    public int PendingCount { get; set; } // الطلبات المعلقة
    public int CompletedCount { get; set; } // الطلبات المكتملة
    public int CancelledCount { get; set; } // الطلبات الملغاة
}

// 3. يستقبل البيانات المرسلة لتحديث حالة الطلب أو رفع ملف مرفق به
public class UpdateOrderAdminRequest
{
    public int OrderId { get; set; }
    public string Status { get; set; } = string.Empty;

    // هنا بنستقبل ملف الصورة أو العقد الفعلي المرفوع زي الكود بتاعكم
    public IFormFile? Attachment { get; set; }
}
