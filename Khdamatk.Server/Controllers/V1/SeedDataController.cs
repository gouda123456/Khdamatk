using Khdamatk.Server.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

/// <summary>
/// تحكم خاص بملء وتجهيز البيانات التجريبية
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[AllowAnonymous]
public class SeedDataController : ControllerBase
{
    private readonly DataSeederService _seederService;
    private readonly ILogger<SeedDataController> _logger;

    public SeedDataController(DataSeederService seederService, ILogger<SeedDataController> logger)
    {
        _seederService = seederService;
        _logger = logger;
    }

    /// <summary>
    /// تشغيل عملية ملء البيانات الشاملة لجميع الـ 35 جدول
    /// 
    /// هذا الـ Endpoint يقوم بـ:
    /// ✅ إنشاء 50 صورة PNG (400x400px) مختلفة الألوان في wwwroot/uploads
    /// ✅ إنشاء 20 مستخدم بكل الأدوار (Admin, Client, Freelancer, ServiceProvider)
    /// ✅ كلمة السر لجميع المستخدمين: Giggo343@
    /// ✅ جميع المستخدمين لديهم EmailConfirmed = true
    /// ✅ ملء جميع الجداول المرتبطة:
    ///    - 8 تصنيفات
    ///    - 30+ مهارة
    ///    - 10 مقدمي خدمة متكاملين
    ///    - 30 خدمة
    ///    - 10 شهادات و50 عمل سابق
    ///    - 15 إعلان وظيفة مع 45 عرض عمل
    ///    - 20 طلب خدمة و10 طلب وظيفة
    ///    - 30+ معاملة مالية
    ///    - 20 بطاقة ائتمانية
    ///    - 30+ محادثة برسائل
    ///    - 15+ تقييم و3+ نزاعات
    ///    - مفضلة ومسلمات وتقارير
    /// </summary>
    /// <remarks>
    /// تشغيل مرة واحدة فقط - لا تشغل الـ Endpoint مرتين على نفس قاعدة البيانات
    /// </remarks>
    [HttpPost("seed-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SeedAllData(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("🚀 بدء عملية ملء البيانات الشاملة...");

            var result = await _seederService.SeedAllDataAsync(cancellationToken);

            if (!result)
            {
                _logger.LogWarning("⚠️ تم اكتشاف بيانات موجودة بالفعل - تم التوقف لتجنب التكرار");
                return Conflict(new
                {
                    statusCode = 409,
                    message = "تم اكتشاف بيانات موجودة بالفعل",
                    arabicMessage = "قاعدة البيانات تحتوي على بيانات موجودة بالفعل. لا يمكن تشغيل ملء البيانات مرة أخرى.",
                    hint = "إذا أردت إعادة التشغيل، قم بحذف البيانات أولاً من قاعدة البيانات"
                });
            }

            _logger.LogInformation("✅ تم ملء البيانات بنجاح!");

            return Ok(new
            {
                statusCode = 200,
                message = "تم ملء البيانات بنجاح",
                arabicMessage = "تم إضافة جميع البيانات التجريبية بنجاح لجميع الـ 35 جدول",
                details = new
                {
                    users = "20 مستخدم (Admin, Client, Freelancer, ServiceProvider)",
                    password = "Giggo343@ لجميع المستخدمين",
                    mediaFiles = "50 صورة PNG في wwwroot/uploads",
                    categories = "8 تصنيفات",
                    skills = "30+ مهارة",
                    serviceProviders = "10 مقدمي خدمة",
                    services = "30 خدمة",
                    certificates = "10 شهادات",
                    portfolioItems = "50 عمل سابق",
                    jobPosts = "15 إعلان وظيفة",
                    jobOffers = "45 عرض عمل",
                    serviceOrders = "20 طلب خدمة",
                    jobOrders = "10 طلب وظيفة",
                    paymentTransactions = "30+ معاملة مالية",
                    creditCards = "20 بطاقة ائتمانية",
                    conversations = "30+ محادثة",
                    messages = "100+ رسالة",
                    reviews = "15+ تقييم",
                    disputes = "3+ نزاعات",
                    userFavorites = "100+ مفضل",
                    jobDeliverables = "10+ مسلم"
                },
                uploadedFiles = "ستجد جميع الصور في: wwwroot/uploads/image_1.png إلى image_50.png"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"❌ خطأ في ملء البيانات: {ex.Message}");
            return StatusCode(500, new
            {
                statusCode = 500,
                message = "حدث خطأ أثناء ملء البيانات",
                arabicMessage = "فشل تنفيذ عملية ملء البيانات",
                error = ex.Message
            });
        }
    }

    /// <summary>
    /// التحقق من حالة ملء البيانات
    /// </summary>
    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSeedStatus()
    {
        return Ok(new
        {
            message = "استخدم POST /api/v1/seeddata/seed-all لبدء ملء البيانات",
            guidelines = new
            {
                step1 = "تأكد من تطبيق Migrations على قاعدة البيانات",
                step2 = "قم بتشغيل الـ Endpoint POST مرة واحدة فقط",
                step3 = "سيتم إنشاء 50 صورة تلقائياً في wwwroot/uploads",
                step4 = "كلمة السر لجميع المستخدمين: Giggo343@",
                step5 = "انتظر قليلاً (قد يستغرق 30-60 ثانية)"
            },
            testCreds = new
            {
                email = "user1@khdamatk.com",
                password = "Giggo343@"
            }
        });
    }
}
