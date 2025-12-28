namespace Khdamatk.Server.Data.Entities.Catalog;

public class Service : BaseEntity
{

    // id, CreatedAt, UpdatedAt from BaseEntity
    // 1. الخصائص الأساسية للخدمة
    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    // 2. خصائص التسعير والمواصفات الأساسية (بدلاً من ServicePackage)
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(1, 100_000_000)] // السعر الأساسي للخدمة
    public decimal Price { get; set; }

    [Range(1, 365)]
    public int DeliveryTimeInDays { get; set; } // وقت التسليم

    [Range(0,5)]
    public double AverageRating { get; set; } = 0;
    public int TotalReviews { get; set; } = 0;


    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(ServiceProviderProfile))]
    public string ServiceProviderProfileId { get; set; } = null!;


    [ForeignKey(nameof(MainImage))]
    public int? MainMediaId { get; set; }



    public virtual Media? MainImage { get; set; }
    
    public virtual Category Category { get; set; } = null!;
    public virtual ServiceProviderProfile ServiceProviderProfile { get; set; } = null!;
    public virtual ICollection<ServiceMedia> MediaGalleryLinks { get; set; } = [];



    //TODO: public virtual ICollection<ServicePackage> Packages { get; set; } FOR LATER IMPLEMENTATION
}


public class ServiceMedia
{
    // المفاتيح التي تشكل المفتاح المركب
    public int ServiceId { get; set; }
    public int MediaId { get; set; }

    // خصائص التنقل
    public virtual Service Service { get; set; } = null!;
    public virtual Media Media { get; set; } = null!;
}