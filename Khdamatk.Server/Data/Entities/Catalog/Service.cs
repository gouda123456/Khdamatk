namespace Khdamatk.Server.Data.Entities.Catalog;

public class Service : BaseEntity
{

    // id, CreatedAt, UpdatedAt from BaseEntity
    // 1. الخصائص الأساسية للخدمة
    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, MinimumLength = 10)]
    public string ShortDescription { get; set; } = string.Empty;
    public string DetailedDescription { get; set; } = string.Empty;

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
    public int RevisionCount { get; set; } = 0;


    // --- الحقول الجديدة المضافة لأهميتها القصوى ---
    public bool IsActive { get; set; } = true; // تمكين/تعطيل الخدمة من قبل الـ Freelancer
    public bool IsApproved { get; set; } = true; // موافقة الإدارة على النشر (لمكافحة السبام)
    public int SalesCount { get; set; } = 0; // لترتيب الخدمات حسب الأكثر مبيعاً
    public int ViewCount { get; set; } = 0; // لقياس مدى رواج الخدمة


    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(ServiceProviderProfile))]
    public string ServiceProviderProfileId { get; set; } = null!;


    [ForeignKey(nameof(MainImage))]
    public int? MainMediaId { get; set; }

    

    public virtual List<string> Concepts { get; set; } = [];

    public virtual Media? MainImage { get; set; }
    
    public virtual Category Category { get; set; } = null!;
    public virtual ServiceProviderProfile ServiceProviderProfile { get; set; } = null!;
    public virtual ICollection<ServiceMedia> MediaGalleryLinks { get; set; } = [];

    public virtual ICollection<ServiceOrder> Orders { get; set; } = [];

    //TODO: public virtual ICollection<ServicePackage> Packages { get; set; } FOR LATER IMPLEMENTATION



    public static List<Service> Data { get; set; } = new List<Service>
    {
        new Service
        {
            
            Title = "Professional Logo Design",
            ShortDescription = "I will create a unique logo that reflects your brand identity.",
            DetailedDescription = "I am a professional graphic designer with extensive experience in logo design. I will work closely with you to understand your vision and transform it into a unique and attractive logo. The service includes 3 different concepts and unlimited revisions until you are completely satisfied.",
            Price = 50,
            DeliveryTimeInDays = 3,
            CategoryId = 1, // افتراضياً: التصميم الجرافيكي
            ServiceProviderProfileId = "SPP1",
            MainMediaId = 1
        },
        new Service
        {
            
            Title = "Simple Website Development",
            ShortDescription = "I will create a simple and responsive website for your business.",
            DetailedDescription = "I am a web developer specializing in building simple and effective websites. I will use the latest technologies to create a site that reflects your brand and provides an excellent user experience. The service includes UI design, backend development, and website deployment.",
            Price = 200,
            DeliveryTimeInDays = 7,
            CategoryId = 2, // افتراضياً: تطوير الويب
            ServiceProviderProfileId = "SPP2",
            MainMediaId = 2
        },
        new Service
        {
            
            Title = "SEO Optimization",
            ShortDescription = "I will optimize your website to rank higher in search engines.",
            DetailedDescription = "I am an SEO specialist with a proven track record of improving website rankings. I will analyze your website, identify areas for improvement, and implement effective SEO strategies to increase your visibility and drive more traffic to your site.",
            Price = 150,
            DeliveryTimeInDays = 5,
            CategoryId = 3, // افتراضياً: التسويق الرقمي
            ServiceProviderProfileId = "SPP3",
            MainMediaId = 3
        },
        new Service
        {
            Title = "Social Media Management",
            ShortDescription = "I will manage your social media accounts and create engaging content.",
            DetailedDescription = "I am a social media manager with experience in creating and executing successful social media strategies. I will manage your accounts, create engaging content, and interact with your audience to build a strong online presence for your brand." ,
            Price = 50,
            DeliveryTimeInDays = 5,
            CategoryId = 3, // افتراضياً: التسويق الرقمي
            ServiceProviderProfileId = "SPP3",
            MainMediaId = 3

        }
    };
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