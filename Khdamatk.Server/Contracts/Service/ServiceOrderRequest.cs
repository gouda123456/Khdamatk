using Khdamatk.Server.Contracts.Fawaterak;

namespace Khdamatk.Server.Contracts.Service;

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
    string CategoryName,
    string ShortDescription,
    string DetailedDescription,
    decimal Price,
    int RevisionCount,
    List<string> Concepts,
    int DeliverTimeInDays,
    ExperienceLevel ExperienceLevel,
    Media? ServiceEnvelope,
    List<IFormFile>? Attachment
);

public class UpdateServiceRequestValidator : AbstractValidator<UpdateServiceRequest>
{
    public UpdateServiceRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الخدمة مطلوب.")
            .MaximumLength(100).WithMessage("العنوان لا يجب أن يتجاوز 100 حرف.");

        RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("يجب تحديد القسم.");

        RuleFor(x => x.ShortDescription)
            .NotEmpty().WithMessage("الوصف المختصر مطلوب.")
            .MaximumLength(200).WithMessage("الوصف المختصر لا يجب أن يتجاوز 200 حرف.");

        RuleFor(x => x.DetailedDescription)
            .NotEmpty().WithMessage("الوصف التفصيلي مطلوب.")
            .MaximumLength(2000).WithMessage("الوصف التفصيلي لا يجب أن يتجاوز 2000 حرف.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("السعر يجب أن يكون أكبر من الصفر.");

        RuleFor(x => x.RevisionCount)
            .GreaterThanOrEqualTo(0).WithMessage("عدد المراجعات يجب أن يكون صفر أو أكثر.");

        RuleFor(x => x.Concepts)
            .NotEmpty().WithMessage("يجب إضافة مفهوم واحد على الأقل.");

        RuleFor(x => x.DeliverTimeInDays)
            .GreaterThan(0).WithMessage("مدة التسليم يجب أن تكون أكبر من الصفر.");

        RuleFor(x => x.ExperienceLevel)
            .IsInEnum().WithMessage("مستوى الخبرة غير صالح.");
    }
}