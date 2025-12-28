namespace Khdamatk.Server.Data.Entities.Catalog;

public class PortfolioItem : BaseEntity
{
    

    [ForeignKey(nameof(ServiceProviderProfile))]
    public string ServiceProviderProfileId { get; set; } = null!;

    // عنوان المشروع
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    // وصف موجز للمشروع
    [Required]
    [StringLength(1000, MinimumLength = 5)]
    public string Description { get; set; } = string.Empty;

    // رابط مباشر 
    public string? ProjectUrl { get; set; }

    // التاريخ الذي تم فيه إنجاز العمل
    public DateTime CompletionDate { get; set; }

    // علاقة Many:Many مع كيان Media لحفظ الصور/الملفات المرتبطة بالمشروع
    public virtual ICollection<PortfolioMedia> ProjectMediaLinks { get; set; } = [];


    public virtual ServiceProviderProfile ServiceProviderProfile { get; set; } = null!;
}

public class PortfolioMedia
{
    // 1. المفاتيح الأجنبية التي تشكل المفتاح المركب
    public int PortfolioItemId { get; set; }
    public int MediaId { get; set; }

    // 2. خصائص التنقل
    public virtual PortfolioItem PortfolioItem { get; set; } = null!;
    public virtual Media Media { get; set; } = null!;
}