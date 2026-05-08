using Khdamatk.Server.Contracts.Fawaterak;

namespace Khdamatk.Server.Contracts.orders;

public record JobOrderResponse(
    int OrderId,
    OrderType OrderType,
    decimal FinalPrice,
    UserOrderModel Customer,
    UserOrderModel Provider,
    JobSummary JobSummary,
    List<OrderChat> Chat,
    DeliverableFiles DeliverableFiles,
    JobMileStone MileStones
    );

public record UserOrderModel(
    string Id,
    string Name,
    string Email,
    byte[] Picture
    );

public record OrderChat(
    int Id,
    string FromUserId,
    string Content,
    DateTime SendAt

    );

public record DeliverableFiles(
    int Id,
    string FileName,
    long Size,
    DeliveredFileStatues FileStatues
    );

public record JobMileStone(
    int Id,
    string Name,
    string Description,
    bool IsCompleted,
    decimal Price
    );
// 1. طلب إضافة أوردر جديد
public record CreateOrderRequest(
    string ProviderId,    // الـ ID بتاع الـ Freelancer
    decimal FinalPrice,   // السعر المتفق عليه
    string JobDescription,
    List<CreateMileStoneRequest> MileStones // لو الشغل متقسم مراحل
);

public record CreateMileStoneRequest(
    string Name,
    string Description,
    decimal Price
);

// 2. طلب رفض الأوردر (ممكن تضيف سبب الرفض لو حابب)
public record RejectOrderRequest(
    string Reason 
);
public record CreateJobOrderRequest(
    int JobPostId,
    int OfferId
);

public record OrderResponse(
    int Id,
    string OrderTitle,       // استعملت OrderTitle بدل Title عشان الأرور اللي كان عندك
    string OrderDescription, // استعملت OrderDescription بدل Description عشان الأرور
    decimal Budget,
    string Status,           // الحالة (Pending, Accepted, etc.)
    DateTime CreatedAt,
    string ProviderName,     // اسم مقدم الخدمة
    string CustomerName      // اسم العميل
);
public record OrderFiltersRequest(
    string? Status, // فلتر بحالة الأوردر
    int PageNumber = 1,
    int PageSize = 10
);

//////S////////
public record GetServicesRequest(
    string? SearchTerm,    // يبحث في الـ Title أو الـ Concepts
    decimal? MinPrice,     // فلتر أقل سعر
    decimal? MaxPrice,     // فلتر أعلى سعر
    int? CategoryId,       // فلتر بالقسم
    string? SortBy         // ترتيب حسب (Price, Rating, Date)
);

public record ServiceSummaryResponse(
    int Id,
    string Title,
    string ShortDescription,
    decimal Price,
    double AverageRating,
    int TotalReviews,
    string CategoryName,
    string ServiceProviderName
);

public record OrderServiceDetailsResponse(
    int Id,
    string Title,
    string ShortDescription,
    string DetailedDescription, // التفاصيل الكاملة هنا
    decimal Price,
    int DeliveryTimeInDays,
    double AverageRating,
    int TotalReviews,
    int RevisionCount,
    string CategoryName,
    string ServiceProviderName,
    string Concepts,            // البيانات اللي ضفناها في الداتابيز
    DateTime CreatedAt
);

public record AddServiceRequest1(
    string Title,
    string ShortDescription,
    string DetailedDescription,
    decimal Price,
    int DeliveryTimeInDays,
    int CategoryId,
    string ServiceProviderProfileId,
    string Concepts
);

public record UpdateServiceRequest(
    string Title,
    string ShortDescription,
    string DetailedDescription,
    decimal Price,
    int DeliveryTimeInDays,
    int CategoryId,
    string Concepts
);