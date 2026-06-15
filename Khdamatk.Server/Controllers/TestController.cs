using System.Globalization;
using Asp.Versioning;
using Bogus;
using Khdamatk.Server.Contracts.Fawaterak;
using Khdamatk.Server.Helper.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using static Khdamatk.Server.Statics.Consts.PermissionsDefault;
using Database = Khdamatk.Server.Data.Database;

namespace Khdamatk.Server.Controllers;

[Route("api/[controller]")]
[ApiController]

public class TestController(
        [FromServices] Database db,
        [FromServices] UserManager<User> userManager,
        [FromServices] IWebHostEnvironment env) : ControllerBase
{

    private readonly Database db = db;
    private readonly UserManager<User> userManager = userManager;
    private readonly IWebHostEnvironment env = env;



    /// <summary>بادئة عناوين/حقول البيانات المزروعة لتمييزها وإعادة التشغيل بأمان.</summary>
    private const string SeedMarker = "[SEED]";
    private const string SeedEmailDomain = "@khdamatk-seed.test";
    private const string SeedCreatedBy = "TestSeed";
    private const string SeedMediaPrefix = "seed_media_";
    private const string SeedOrderMarker = "SEED_ORDER|";
    
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("API is working!");
    }

    [HttpGet]
    [Authorize]
    [Route("authorized")]
    public IActionResult GetAuthorized()
    {
        return Ok($"{HttpContext.User.GetUserId()}You are authorized!");
    }

    [HttpGet]
    [PermissionAuthorize(PermissionsDefault.WeatherForecast.Modify)]
    [Route("permission")]
    public IActionResult Getpermission()
    {
        return Ok($"{HttpContext.User.GetUserId()}You are authorized!");
    }

    [HttpGet("send-reset-Password")]
    public IActionResult SendResetEmail([FromServices] IEmailHelper emailHelper)
    {
        emailHelper.SendresetPasswordEmailAsync("giggo343@gmail.com", 666666);
        return Ok();
    }


    [HttpGet("test-Einvoice-Payment")]
    public async Task<IActionResult> TestEInvoicePayment([FromServices] IFawaterakPaymentHelper fawaterakPaymentHelper)
    {


        var response = await fawaterakPaymentHelper.CreateEInvoiceAsync(new EInvoiceRequestModel
        {
            Customer = new CustomerModel
            {
                FirstName = "Gouda",
                LastName = "George",
                CustomerId = "123456",
                Email = "giggo343@gmail.com"
            },
            CartItems = new List<CartItemModel>
            {
                new CartItemModel
                {
                    Name = "Product 1",
                    Quantity = 2,
                    Price = 50
                },
                new CartItemModel
                {
                    Name = "Product 2",
                    Quantity = 1,
                    Price = 100
                }
            },
            Currency = "EGP",
            SendEmail = true,
            RedirectionUrls = new RedirectionUrlsModel()
            {
                OnFailure = "https://www.facebook.com",
                OnPending = "https://www.w3schools.com/cs/cs_math.php",
                OnSuccess = "https://learn.microsoft.com/ar-sa/aspnet/core/?view=aspnetcore-8.0&utm_source=aspnet-start-page&utm_campaign=vside"
            },
            Status = OrderStatus.PendingPayment,
            DueDate = DateTime.UtcNow.AddDays(7),
            PayLoad = new InvoicePayload
            {
                OrderId = 1,
                OrderType = OrderType.Service,
                Provider = new ProviderModel
                {
                    Id = "654321",
                    Username = "Provider Name",
                    Email = "godegeorge07@gmail.com"
                }
            }
        });

        return Ok(response);
    }

    [HttpGet("test-file-Download")]
    public async Task<IActionResult> TestFileDownload()
    {
        var media = db.Medias.FirstOrDefault();
        var path = media.FullPath;
        return Ok(path);
    }

    [HttpPost("test-file-upload")]
    public async Task<IActionResult> TestFileUploadPost(IFormFile file)
    {
        var media = await FileManagement.UploadFileAsync(file);
        db.Medias.Add(media);
        await db.SaveChangesAsync();
        return Ok(media);
    }




    [HttpPost("FixSeedData")]
    public async Task<IActionResult> FixSeedData()
    {
        await FixSeedingProblems();
        return Ok("Seed data issues fixed successfully.");
    }

    [HttpPost("SeedData")]
    public async Task<IActionResult> SeedData()
    {
        try
        {


            
            
            var msg = await InjectData();



            // الحصول على الـ ServiceProvider لإنشاء Scope جديد في الخلفية


            return Ok(msg);


            

        //    return Ok(new
        //    {
        //        Message = "Seed completed (idempotent). Safe to call multiple times.",
        //        Notes =
        //            $"{SeedMarker} marks seeded rows. Investigation entity is not mapped to EF. DeliveredJobFile is included when milestones exist.",
        //        Stats = new
        //        {
        //            Users = users.Count,
        //            Categories = categories.Count,
        //            //Skills = skills.Count,
        //            Providers = providers.Count,
        //            //Services = services.Count,
        //            JobPosts = jobPosts.Count,
        //            JobOrders = jobOrders.Count,
        //            //JobDeliverables = jobDeliverables.Count,
        //            //ServiceOrders = serviceOrders.Count,
        //            Media = mediaList.Count,
        //            //JobSkillRequirements = await db.JobSkillRequirements.CountAsync(j => jobPosts.Select(p => p.Id).Contains(j.JobPostId)),
        //            //Reviews = await db.Reviews.CountAsync(r => r.Title.StartsWith(SeedMarker))
        //        }
        //    });
        }
        catch (DbUpdateException dbEx)
        {
            return BadRequest(new
            {
                Error = dbEx.Message,
                Detail = dbEx.InnerException?.Message,
                Type = nameof(DbUpdateException)
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message, Detail = ex.InnerException?.Message, Type = ex.GetType().Name });
        }
    }

    private async Task FixSeedingProblems()
    {
        foreach (var user in await db.Users.ToListAsync())
        {
            int i = 1;
            user.ProfilePictureId = user.ProfilePictureId ?? i;
            i = (i <= 30) ? i : 1;
        }

        await db.SaveChangesAsync();
    }


    private async Task<object> InjectData()
    {
        try
        {
            //MY WORK
            //Identity
            await SeedImagesUsingSystemFilesAsync();
            var mediaList = await SyncMediaAsync();
            

            var roles = await AddRolesAsync();
            var skills = await InjectSkillsAsync();
            await InjectCertificatesAsync(mediaList);

            var users = await InjectUsersAndServiceProviderWithMediaAsync(mediaList, skills.Select(s => s.Id).ToList());
            await AddRolesToUsersAndProviders(users);
            var providers = await db.ServiceProviderProfiles.Include(p => p.User).ToListAsync();
            var admins = await InjectUsersAdminsAsync(mediaList, skills.Select(s => s.Id).ToList());

            //verification Data, portfolio (item , media), Skills, provider Skills 

            //Jop Posts Domain
            var categories = await GetOrCreateSeedCategoriesAsync();

            

            var jobPosts = await InjectJobPostsAsync(users.Where(u => !u.IsServiceProvider).Select(u => u.Id).ToList(), categories.Select(c => c.Id).ToList(), mediaList, skills.Select(s => s.Id).ToList());
            await InjectOffersForJobPostsAsync(jobPosts, mediaList);

            var jobOrders = await InjectJobOrdersFullCycleAsync(jobPosts);


            //Service Domain
            var services = await InjectServicesFullGraphAsync();
            var serviceOrders = await InjectServiceOrdersAsync();


            //Reviews, disputes, 
            var Disputes = await InjectDisputesAsync(serviceOrders.Select(so => so.Id).ToList(),jobOrders.Select(jo => jo.Id).ToList());
            var reviews = await InjectReviewsAsync();

            //milstones, deliveredFiles, skill requirements,






            

            //END OF MY WORK


            //var skills = await GetOrCreateSeedSkillsAsync(db);

            //var services = await GetOrCreateSeedServicesAsync(db, providers, categories, mediaList);

            //await GetOrCreateSeedJobSkillRequirementsAsync(db, jobPosts, skills);
            //var serviceOrders = await GetOrCreateSeedServiceOrdersAsync(db, services, users, providers);
            //await GetOrCreateSeedReviewsAsync(db, serviceOrders, providers);

            //var jobDeliverables = await GetOrCreateSeedJobDeliverablesAsync(db, jobOrders, mediaList);
            //await EnsureSeedExtensionEntitiesAsync(db, users, services, serviceOrders, jobPosts, mediaList);

            return new
            {
                Users = users.Count,
                Categories = categories.Count,
                Skills = skills.Count,
                Providers = providers.Count,
                Services = services.Count,
                JobPosts = jobPosts.Count,
                JobOrders = jobOrders.Count,
                ServiceOrders = serviceOrders.Count,
                Media = mediaList.Count,
                JobSkillRequirements = await db.JobSkillRequirements.CountAsync(j => jobPosts.Select(p => p.Id).Contains(j.JobPostId)),
                Reviews = reviews.Count,
                Disputes = Disputes.Count
            };
        }
        catch (DbUpdateException dbEx)
        {
            return new
            {
                Error = dbEx.Message,
                Detail = dbEx.InnerException?.Message,
                Type = nameof(DbUpdateException)
            };
        }
        catch (Exception ex)
        {
            return new { Error = ex.Message, Detail = ex.InnerException?.Message, Type = ex.GetType().Name };
        }

    }

    #region Helper Methods for Seeding

    //MY WORK:::

    [NonAction]
    public async Task<List<Media>> SeedImagesUsingSystemFilesAsync(int count = 100)
    {
        
        var Dictionary = Directory.GetFiles(FileManagement.MediaPath);
        if(Dictionary.Length >= 250)
        {
            Console.WriteLine($"[FileManagement] Found {Dictionary.Length} existing files. Skipping download.");
            return await db.Medias.ToListAsync();
        }

        var client = new HttpClient();
        var mediaEntities = new List<Media>();

        for (int i = 1; i <= count; i++)
        {
            try
            {
                // 1. تحميل الصورة من الإنترنت
                var imageUrl = $"https://picsum.photos/400/400?random={i}";
                var imageBytes = await client.GetByteArrayAsync(imageUrl);

                // 2. تحويل الـ Bytes إلى FormFile لمحاكاة رفع ملف حقيقي
                var fileName = $"profile_seed_{i}{Guid.CreateVersion7().ToString()}.jpg".Replace(" ","");
                var stream = new MemoryStream(imageBytes);

                var formFile = new FormFile(stream, 0, imageBytes.Length, "file", fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                };

                // 3. استخدام ميثود الـ FileManagement الخاصة بك
                // ملاحظة: بما أنها Static نستخدم اسم الكلاس مباشرة
                var media = await FileManagement.UploadFileAsync(formFile);

                if (media != null)
                {
                    mediaEntities.Add(media);
                    Console.WriteLine($"[FileManagement] Processed: {fileName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding image {i}: {ex.Message}");
            }
        }

        // 4. حفظ الكائنات في قاعدة البيانات ليكون لها ID
        if (mediaEntities.Any())
        {
            await db.Medias.AddRangeAsync(mediaEntities);
            await db.SaveChangesAsync();
        }

        return mediaEntities;
    }



    [NonAction]
    public async Task<List<Media>> SyncMediaAsync()
    {
        // 1. جلب كل الأسماء الموجودة حالياً في الداتا بيز
        var existingNames = await db.Medias
            .Select(m => m.FileName)
            .ToListAsync();

        // 2. مناداة ميثود المزامنة
        var newMedia = FileManagement.SyncFolderWithDatabase(existingNames);

        // 3. الحفظ في الداتا بيز إذا وجد جديد
        if (newMedia.Any())
        {
            db.Medias.AddRange(newMedia);
            await db.SaveChangesAsync();
        }

        return db.Medias.ToList();
    }
    
    [NonAction]
    public async Task<List<Role>> AddRolesAsync()
    {
        if (await db.Roles.CountAsync() >= 3)
            return await db.Roles.ToListAsync();

        await db.Roles.AddRangeAsync(
            new Role
            {
                Id = "1",
                Name = RolesStrings.Admin,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new Role
            {
                Id = "2",
                Name = RolesStrings.Member,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new Role
            {
                Id = "3",
                Name = RolesStrings.ServiceProvider,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        );
        await db.SaveChangesAsync();
        return await db.Roles.ToListAsync();
    }

    [NonAction]
    public async Task<List<User>> InjectUsersAndServiceProviderWithMediaAsync(List<Media> medias,List<int> skillIds, int count = 50)
    {


        try
        {

        
        

        // 3. توليد المستخدمين وربطهم عشوائياً بالميديا
        var passwordHasher = new PasswordHasher<User>();
        var newUsers = GetUserFaker(count, medias.Select(m => m.Id).ToList(), passwordHasher);

        // 3. تحديد من سيصبح فري لانسر (70%)
        var random = new Random();
        var shuffledUsers = newUsers.OrderBy(x => random.Next()).ToList();
        int providerCount = (int)(count * 0.7);

        var providersToCreate = new List<ServiceProviderProfile>();

        for (int i = 0; i < shuffledUsers.Count; i++)
        {
            var user = shuffledUsers[i];

            if (i < providerCount)
            {
                // هذا المستخدم فري لانسر
                

                // إنشاء بروفايل له
                var profileFaker = GetProfileWithPortfolioFaker(user.Id, skillIds, medias);
                providersToCreate.Add(profileFaker.Generate());
            }
            else
            {
                // هذا المستخدم عميل فقط
                
            }
        }

            foreach (var user in await db.Users.ToListAsync())
            {
                user.ProfilePictureId = user.ProfilePictureId ?? 1;
            }
            

        // 4. الحفظ في قاعدة البيانات
        await db.Users.AddRangeAsync(newUsers);
        await db.ServiceProviderProfiles.AddRangeAsync(providersToCreate);

        await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }

        return await db.Users.ToListAsync();

    }

    [NonAction]
    public  async Task<List<User>> InjectUsersAdminsAsync(List<Media> medias, List<int> skillIds, int count = 10)
    {
        // 3. توليد المستخدمين وربطهم عشوائياً بالميديا
        var passwordHasher = new PasswordHasher<User>();
        var newUsers = GetUserFaker(count, medias.Select(m => m.Id).ToList(), passwordHasher);

        await db.Users.AddRangeAsync(newUsers);

        var adminRoleId = (await db.Roles.FirstAsync(r => r.Name == RolesStrings.Admin)).Id;

        await db.UserRoles.AddRangeAsync(newUsers.Select(u => new IdentityUserRole<string>
        {
            UserId = u.Id,
            RoleId = adminRoleId
        }));

        await db.SaveChangesAsync();


        foreach (var user in await db.Users.Where(u => u.ProfilePictureId == null).ToListAsync())
        {
            int i = 1;
            user.ProfilePictureId = i;
            i = (i <= 10) ? i : 1;
        }

        await db.SaveChangesAsync();

        return await db.Users.ToListAsync();
    }

    [NonAction]
    public static Faker<ServiceProviderProfile> GetProfileWithPortfolioFaker(
    string userId,
    List<int> skillIds,
    List<Media> medias) // أضفنا قائمة الميديا هنا لربطها بمعرض الأعمال
    {
        var random = new Random();
        var mediaIds = medias.Select(m => m.Id).ToList();

        return new Faker<ServiceProviderProfile>("en")
            .RuleFor(p => p.UserId, userId)
            .RuleFor(p => p.JobTitle, f => f.Name.JobTitle().ClampLength(2, 50))
            .RuleFor(p => p.Bio, f => f.Lorem.Paragraph().ClampLength(10, 1000))

            .RuleFor(p => p.ExperienceYears, f => f.Random.Number(1, 15))
            .RuleFor(p => p.HourlyRate, f => Math.Round(f.Random.Double(10, 200), 2))
            .RuleFor(p => p.WorkingHoursPerWeek, f => Math.Round(f.Random.Double(10, 60), 1))
            .RuleFor(p => p.IsActive, true)
            .RuleFor(p => p.IsAvailable, f => f.Random.Bool(0.9f))

            .RuleFor(p => p.FacebookUrl, f => f.Internet.Url())
            .RuleFor(p => p.GithubUrl, f => f.Internet.Url())
            .RuleFor(p => p.LinkedInUrl, f => f.Internet.Url())
            .RuleFor(p => p.TwitterUrl, f => f.Internet.Url())

            .RuleFor(p => p.TotalReviews, f => f.Random.Number(0, 50))
            .RuleFor(p => p.AverageRating, f => Math.Round(f.Random.Double(3, 5), 1))
            .RuleFor(p => p.CompletedJobs, f => f.Random.Number(0, 100))
            .RuleFor(p => p.AverageResponseTime, f => f.Random.Number(1, 24))
            .RuleFor(p => p.DateOfJoin, f => f.Date.Past(1))

            // 1. توليد وحقن مهارات مقدم الخدمة (ProviderSkills)
            .RuleFor(p => p.Skills, f => {
                var selectedSkills = new List<ProviderSkill>();
                if (skillIds != null && skillIds.Any())
                {
                    var randomSkillIds = f.PickRandom(skillIds, f.Random.Number(2, 5)).Distinct().ToList();
                    foreach (var id in randomSkillIds)
                    {
                        selectedSkills.Add(new ProviderSkill
                        {
                            SkillId = id,
                            MyLevel = f.PickRandom<SkillExperienceLevel>()
                        });
                    }
                }
                return selectedSkills;
            })

            // 2. توليد وحقن معرض الأعمال (PortfolioItems) مع الـ PortfolioMedia التابعة
            .RuleFor(p => p.PortfolioItems, f => {
                var portfolioList = new List<PortfolioItem>();
                int projectsCount = f.Random.Number(1, 3); // توليد من مشروع إلى 3 مشاريع لكل شخص

                for (int i = 0; i < projectsCount; i++)
                {
                    // اختيار من 1 إلى 2 ميديا فريدة عشوائياً لكل مشروع في المعرض
                    var selectedMediaIds = f.PickRandom(mediaIds, f.Random.Number(1, 2)).Distinct().ToList();

                    var portfolioItem = new PortfolioItem
                    {
                        Title = f.Commerce.ProductName().ClampLength(2, 50),
                        Description = f.Lorem.Paragraph().ClampLength(5, 1000),
                        ProjectUrl = f.Internet.Url(),
                        CompletionDate = f.Date.Past(2),

                        // بيانات إضافية اختيارية لمحاكاة الواقعية
                        SchoolName = f.Company.CompanyName().ClampLength(0, 100),
                        Degree = f.Lorem.Word().ClampLength(0, 50),
                        FieldOfStudy = f.Lorem.Word().ClampLength(0, 50),
                        Company = f.Company.CompanyName().ClampLength(0, 100),
                        StartDate = f.Date.Past(4),
                        EndDate = f.Date.Past(2)
                    };

                    // ربط الميديا بالمشروع الحالي عبر كائن الوسيط PortfolioMedia
                    portfolioItem.ProjectMediaLinks = selectedMediaIds.Select(mId => new PortfolioMedia
                    {
                        MediaId = mId
                    }).ToList();

                    portfolioList.Add(portfolioItem);
                }

                return portfolioList;
            });
    }


    [NonAction]
    public static List<User> GetUserFaker(int count, List<int> availableMediaIds, IPasswordHasher<User> passwordHasher)
    {
        

        var userFaker = new Faker<User>("en")
            .RuleFor(u => u.Id, f => Guid.CreateVersion7().ToString())
            .RuleFor(u => u.FullName, f => f.Name.FirstName() + " " + f.Name.LastName())
            .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FullName).ToLower())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FullName).ToLower())
            .RuleFor(u => u.Amount, f => f.Finance.Amount(100, 10000))
            .RuleFor(u => u.DateOfBirth, f => f.Date.Past(50, DateTime.UtcNow.AddYears(-18)))
            .RuleFor(u => u.IsTrustedByAdmin, f => true) // 30% chance to be trusted
            .RuleFor(u => u.IsVerified, f => true) // 70% chance to be verified
            // تأكيد الحسابات
            .RuleFor(u => u.EmailConfirmed, true)
            .RuleFor(u => u.PhoneNumberConfirmed, true)
            .RuleFor(u => u.TwoFactorEnabled, false)
            // ربط الميديا (صورة البروفايل)
            .RuleFor(u => u.ProfilePictureId, f => f.PickRandom(availableMediaIds))
            .RuleFor(u => u.VerificationData, f => new VerificationData
            {
                City = f.Address.City().ClampLength(2, 50),
                Country = f.Address.Country().ClampLength(2, 50),
                NationalNumber = f.Random.Replace("#########"), // رقم وطني مكون من 9 أرقام
                Status = f.PickRandom<VerificationStatus>()
            })
            .FinishWith((f, u) =>
            {
                // تشفير كلمة السر المطلوبة
                //u.ProfilePictureId = f.PickRandom(availableMediaIds);
                if (u.ProfilePictureId == null)
                    u.ProfilePictureId = 1;

                u.PasswordHash = passwordHasher.HashPassword(u, "Giggo343@");
            });

        return userFaker.Generate(count);
    }


    


    [NonAction]
    public async Task AddRolesToUsersAndProviders(List<User> users)
    {
        var memberRoleId = (await db.Roles.FirstAsync(r => r.Name == RolesStrings.Member)).Id;
        var providerRoleId = (await db.Roles.FirstAsync(r => r.Name == RolesStrings.ServiceProvider)).Id;

        // 1. جلب كل العلاقات الموجودة حالياً في القاعدة لتجنب التكرار
        var existingUserRoles = await db.UserRoles.ToListAsync();

        var newUserRoles = new List<IdentityUserRole<string>>();

        foreach (var user in users)
        {
            var targetRoleId = user.IsServiceProvider ? providerRoleId : memberRoleId;

            // 2. التحقق: هل هذا المستخدم لديه هذا الدور فعلاً؟
            bool alreadyHasRole = existingUserRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == targetRoleId);

            if (!alreadyHasRole)
            {
                newUserRoles.Add(new IdentityUserRole<string>
                {
                    UserId = user.Id,
                    RoleId = targetRoleId
                });
            }
        }

        // 3. إضافة العلاقات الجديدة فقط
        if (newUserRoles.Any())
        {
            await db.UserRoles.AddRangeAsync(newUserRoles);
            await db.SaveChangesAsync();
        }
    }


    [NonAction]
    private async Task<List<Category>> GetOrCreateSeedCategoriesAsync()
    {
        if (await db.Categories.CountAsync() >= 6)
            return await db.Categories.Take(6).ToListAsync();

        var definitions = new (string Name, string Description)[]
        {
            ("Web Development", "Services for web development and application development"),
            ("Mobile Development", "Services for mobile app development"),
            ("UI/UX Design", "User interface and user experience design services"),
            ("Graphic Design", "Graphic design and branding services"),
            ("Digital Marketing", "Digital marketing and social media management"),
            ("Content Writing", "Content creation and copywriting services"),
            ("Video Editing", "Video editing and post-production services"),
            ("Data Analysis", "Data analysis and visualization services"),
            ("Cloud Services", "Cloud computing and infrastructure services"),
            ("Cyber Security", "Cybersecurity and data protection services"),
            ("AI & Machine Learning", "Artificial intelligence and machine learning services"),
            ("Business Consulting", "Business strategy and consulting services"),
            ("Translation Services", "Language translation and localization services"),
            ("Financial Services", "Financial consulting and accounting services"),
            ("Legal Services", "Legal advice and document preparation services"),
            ("Health & Wellness", "Health coaching and wellness services"),
            ("Education & Tutoring", "Educational content and tutoring services"),
            ("Photography", "Professional photography services"),
            ("Video Production", "Full video production services"),
            ("Voice Over", "Voice over and narration services"),
            ("Music & Audio", "Music production and audio editing services"),
            ("Virtual Assistance", "Virtual assistant and administrative support services"),
            ("Programming & Tech", "Programming, tech support, and IT services"),
            ("Writing & Translation", "Writing, editing, and translation services"),
            ("Design & Creative", "Graphic design, video editing, and creative services"),
            ("Digital Marketing", "SEO, social media marketing, and advertising services"),
            ("Business & Consulting", "Business strategy, consulting, and financial services"),
            ("Lifestyle Services", "Health coaching, fitness training, and lifestyle services")
        };

        var list = new List<Category>();
        foreach (var (name, desc) in definitions)
        {
            var c = await db.Categories.FirstOrDefaultAsync(x => x.Name == name);
            if (c == null)
            {
                c = new Category { Name = name, Description = desc };
                db.Categories.Add(c);
                await db.SaveChangesAsync();
            }

            list.Add(c);
        }

        return list;
    }

    [NonAction]
    public async Task InjectCertificatesAsync(List<Media> medias)
    {
        try
        {
            // 1. جلب قائمة مقدمي الخدمة المتاحين
            var providerIds = await db.ServiceProviderProfiles
                .Select(p => p.UserId)
                .ToListAsync();

            if (!providerIds.Any()) return;

            // جلب معرفات الميديا المتاحة (صور الشهادات)
            var mediaIds = medias.Select(m => m.Id).ToList();

            var allCertificates = new List<Certificate>();
            var random = new Random();

            // 2. لكل مقدم خدمة، سنقوم بتوليد من 1 إلى 3 شهادات
            foreach (var providerId in providerIds)
            {
                var certificateFaker = GetCertificateFaker(providerId, mediaIds);

                // توليد عدد عشوائي من الشهادات لمقدم الخدمة هذا
                var count = random.Next(1, 4);
                var certsForThisProvider = certificateFaker.Generate(count);

                allCertificates.AddRange(certsForThisProvider);
            }

            // 3. الحفظ في قاعدة البيانات
            if (allCertificates.Any())
            {
                await db.Certificates.AddRangeAsync(allCertificates);
                await db.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding certificates: {ex.Message}");
            throw;
        }
    }

    [NonAction]
    public static Faker<Certificate> GetCertificateFaker(string providerId, List<int> mediaIds)
    {
        // قائمة بأنواع الشهادات لتبدو البيانات واقعية
        var certTypes = new[] { "Professional", "Academic", "Technical", "Specialization", "Workshop" };

        return new Faker<Certificate>("en")
            // ربط الشهادة بمقدم الخدمة
            .RuleFor(c => c.ServiceProviderProfileId, providerId)

            // توليد عنوان شهادة (مثل: Microsoft Certified Expert)
            .RuleFor(c => c.Title, f => f.Commerce.ProductName().ClampLength(2, 50))

            // الجهة المانحة (مثل: Google, Coursera, MIT)
            .RuleFor(c => c.Issuer, f => f.Company.CompanyName())

            // نوع الشهادة
            .RuleFor(c => c.Type, f => f.PickRandom(certTypes))

            // سنة الحصول على الشهادة (من 2010 حتى الآن)
            .RuleFor(c => c.YearAcquired, f => f.Date.Past(15).Year)

            // ربط صورة الشهادة (Media) بشكل عشوائي إذا وجدت
            .RuleFor(c => c.MediaId, f => mediaIds.Any() ? f.PickRandom(mediaIds) : (int?)null);
    }

    [NonAction]
    public async Task<List<Skill>> InjectSkillsAsync()
    {
        try
        {
            // 1. التأكد من وجود المهارات الأساسية في قاعدة البيانات أولاً
            var existingSkills = await db.Skills.ToListAsync();
            if (!existingSkills.Any())
            {
                await db.Skills.AddRangeAsync(Skill.Data);
                await db.SaveChangesAsync();
                existingSkills = await db.Skills.ToListAsync();
            }
            
            return existingSkills;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding skills: {ex.Message}");
            Console.WriteLine($"Error seeding skills: {ex.InnerException?.Message}");
            throw;
        }
    }

    


    //Jop Posts Domain

    [NonAction]
    public async Task<List<JobPost>> InjectJobPostsAsync(List<string> customerIds, List<int> categoryIds, List<Media> medias, List<int>? skillIds, int count = 50)
    {

        try
        {
             
            var jobPostFaker = GetJobPostFaker(customerIds, categoryIds, medias, skillIds);
            var newJobs = jobPostFaker.Generate(count);

            await db.JobPosts.AddRangeAsync(newJobs);
            await db.SaveChangesAsync();

            return newJobs;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        
        
    }


    [NonAction]
    public static Faker<JobPost> GetJobPostFaker(List<string> customerIds, List<int> categoryIds, List<Media> medias, List<int> skillIds)
    {
        try
        {

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding job posts: {ex.Message}");
            if(ex.InnerException != null)
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            throw;
        }
        return new Faker<JobPost>("en")
            // العنوان: نستخدم ميثod توفر جملة بطول محدد بدلاً من Substring اليدوي الخطير
            .RuleFor(j => j.Title, f => f.Lorem.Sentence(3).LimitLength(100))

            // الوصف: نفس الشيء، نستخدم ميثod آمنة
            .RuleFor(j => j.Description, f => f.Lorem.Paragraphs(2).LimitLength(1000))

            .RuleFor(j => j.BudgetMin, f => f.Finance.Amount(50, 500))
            .RuleFor(j => j.BudgetMax, (f, j) => j.BudgetMin + f.Finance.Amount(50, 5000))

            .RuleFor(j => j.Status, f => f.PickRandom<JobPostStatus>())
            .RuleFor(j => j.ExperienceLevel, f => f.PickRandom<ExperienceLevel>())
            .RuleFor(j => j.TimeCommitment, f => f.PickRandom<TimeCommit>())

            .RuleFor(j => j.ProjectLength, f => f.PickRandom(new[] { "Less than 1 month", "1-3 months", "3-6 months", "More than 6 months" }))
            .RuleFor(j => j.Deadline, f => f.Date.Soon(30))
            .RuleFor(j => j.CreatedAt, f => f.Date.Past(1))

            .RuleFor(j => j.CustomerId, f => f.PickRandom(customerIds))
            .RuleFor(j => j.CategoryId, f => f.PickRandom(categoryIds))

            // بما أنك ألغيت الصرامة عالمياً، يمكنك الآن اختيار صور عشوائية حتى لو تكررت
            .RuleFor(j => j.Media, f => f.PickRandom(medias, f.Random.Number(1, 3)).ToList())
            .RuleFor(j => j.SkillRequirements, f => {
                // اختيار من 1 إلى 3 مهارات فريدة عشوائياً لهذا المنشور لمنع تكرار الـ Primary Key المركب
                var selectedSkillIds = f.PickRandom(skillIds, f.Random.Number(1, 3)).Distinct().ToList();

                return selectedSkillIds.Select(skillId => new JobSkillRequirement
                {
                    SkillId = skillId,
                    RequiredLevel = f.PickRandom<SkillExperienceLevel>() // توليد مستوى الخبرة المطلوب عشوائياً
                }).ToList();
            })
            .RuleFor(j => j.DeliveredFiles, f => new List<DeliveredJobFile>
        {
            new DeliveredJobFile
            {
                // نختار MediaId من الموجودين في النظام
                MediaId = f.PickRandom(medias.Select(m => m.Id).ToList()),
                Statues = f.PickRandom<DeliveredFileStatues>(),
                
                // توليد الـ MileStone التابع لملف التسليم في نفس اللحظة
                MileStone = new MileStone
                {
                    Title = f.Commerce.ProductName().ClampLength(2, 150),
                    Description = f.Lorem.Paragraph().ClampLength(10, 1000),
                    StepNumber = 1,
                    IsCompleted = f.Random.Bool(0.5f),
                    Price = Math.Round(f.Finance.Amount(50, 1000), 2)
                }
            }
        });
    }

    
    



    [NonAction]
    public async Task InjectOffersForJobPostsAsync(List<JobPost> jobPosts, List<Media> medias)
    {
        try
        {
            // 1. جلب قائمة الـ Providers المتاحة في النظام لربط العروض بهم
            var providerIds = await db.ServiceProviderProfiles
                .Select(p => p.UserId)
                .ToListAsync();

            if (!providerIds.Any()) return;

            var allOffers = new List<JobOffer>();
            var random = new Random();

            // 2. لكل JobPost، سنقوم بتوليد من 3 إلى 8 عروض
            foreach (var job in jobPosts)
            {
                var offerFaker = GetJobOfferFaker(job.Id, providerIds, medias);

                // توليد عدد عشوائي من العروض لهذا المنشور
                var offersForThisJob = offerFaker.Generate(random.Next(3, 8));

                allOffers.AddRange(offersForThisJob);
            }

            // 3. إضافة كل العروض دفعة واحدة لقاعدة البيانات لتحسين الأداء
            if (allOffers.Any())
            {
                await db.JobOffers.AddRangeAsync(allOffers);
                await db.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine(ex.InnerException?.Message);

            throw;
        }

        
    }



    [NonAction]
    public static Faker<JobOffer> GetJobOfferFaker(int jobPostId, List<string> providerIds,List<Media> medias)
    {
        try
        {
             return new Faker<JobOffer>("en")
                        .RuleFor(o => o.JobPostId, jobPostId)
                        // اختيار فريلانسر عشوائي من القائمة المتاحة
                        .RuleFor(o => o.ProviderProfileId, f => f.PickRandom(providerIds))
                        .RuleFor(o => o.Description, f => f.Lorem.Text().LimitLength(100))
                        .RuleFor(o => o.DeliveryTimeInDays, f => f.Random.Number(1, 30))
                        .RuleFor(o => o.Amount, f => f.Finance.Amount(50, 5000))
                        .RuleFor(o => o.SimilarWorkExamplesURL, f => f.Internet.Url())
                        .RuleFor(o => o.Status, JobOfferStatus.Pending)
                        .RuleFor(o => o.IsAccepted, false)
                        .RuleFor(o => o.ExperienceLevel, f => f.PickRandom<ExperienceLevel>())
                        .RuleFor(o => o.TimeCommitment, f => f.PickRandom<TimeCommit>())
                        .FinishWith((f,o) =>
                        {
                            o.Attachments = f.PickRandom(medias, f.Random.Number(0, 3)).ToList();
                        });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine(ex.InnerException?.Message);

            throw;
        }
       
    }

    [NonAction]
    public async Task<List<JobOrder>> InjectJobOrdersFullCycleAsync(List<JobPost> jobPosts)
    {
        try
        {
            var newOrders = new List<JobOrder>();
            var random = new Random();
            var faker = new Faker("en");

            // 1. تعريف المصفوفات هنا في الأعلى لضمان وصول جميع أجزاء الكود لها
            string[] customerSamples = {
            "Hello, I've accepted your offer. Let's start!",
            "When can you provide the first draft?",
            "I've shared the requirements, please confirm receipt."
        };

            string[] providerSamples = {
            "Thank you for choosing me! I'll start immediately.",
            "I will send you an update by tomorrow.",
            "Received, I'm working on it now."
        };

            foreach (var job in jobPosts)
            {
                var winningOffer = await db.JobOffers
                    .Where(o => o.JobPostId == job.Id)
                    .OrderBy(o => Guid.NewGuid())
                    .FirstOrDefaultAsync();

                if (winningOffer == null) continue;

                winningOffer.IsAccepted = true;
                winningOffer.Status = JobOfferStatus.Accepted;
                job.Status = JobPostStatus.InProgress;

                // 2. بناء الطلب (BuildOrder)
                var order = JobOrder.BuildOrder(job, winningOffer);

                // 3. ملء المحادثة بالرسائل
                if (order.Conversation != null)
                {
                    var messages = new List<Message>();
                    int messageCount = random.Next(3, 6);

                    for (int i = 0; i < messageCount; i++)
                    {
                        bool isCustomerSender = i % 2 == 0;
                        messages.Add(new Message
                        {
                            Conversation = order.Conversation,
                            SenderId = isCustomerSender ? job.CustomerId : winningOffer.ProviderProfileId,
                            Content = isCustomerSender
                                      ? faker.PickRandom(customerSamples)
                                      : faker.PickRandom(providerSamples),
                            CreatedAt = DateTime.UtcNow.AddMinutes(i * 15),
                            IsRead = true
                        });
                    }
                    order.Conversation.Messages = messages;
                }

                // 4. العملية المالية
                var transaction = new PaymentTransaction
                {
                    JobOrder = order,
                    Amount = order.Amount,
                    PlatformFee = order.Amount * 0.15m,
                    NetPayout = order.Amount - (order.Amount * 0.15m),
                    Currency = CurrencyCode.EGP,
                    Status = TransactionStatus.Completed,
                    GatewayUsed = faker.PickRandom<PaymentGateway>(),
                    TransactionDate = DateTime.UtcNow.AddMinutes(-30),
                    GatewayReferenceId = "PAY-" + faker.Random.AlphaNumeric(12).ToUpper()
                };

                order.PaymentTransaction = transaction;
                newOrders.Add(order);
            }

            // 5. الحفظ النهائي
            if (newOrders.Any())
            {
                await db.JobOrders.AddRangeAsync(newOrders);
                await db.SaveChangesAsync();
            }

            return newOrders;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }





    //Service
    [NonAction]
    public async Task<List<Service>> InjectServicesFullGraphAsync(int countToGenerate = 20)
    {
        try
        {
            // 1. جلب معرفات مقدمي الخدمة المتاحين
            var profileIds = await db.ServiceProviderProfiles.Select(p => p.UserId).ToListAsync();

            // 2. جلب معرفات الأقسام (Categories) المتاحة في قاعدة البيانات
            var categoryIds = await db.Categories.Select(c => c.Id).ToListAsync();

            // 3. جلب كائنات الميديا المتاحة
            var medias = await db.Medias.ToListAsync();

            // تحقق دفاعي صارم لضمان عدم حدوث تعارض في الـ Foreign Keys
            if (!profileIds.Any() || !categoryIds.Any() || !medias.Any())
            {
                Console.WriteLine("Warning: Seeding services skipped. Ensure Profiles, Categories, and Medias tables have data.");
                return new List<Service>();
            }

            var newServices = new List<Service>();
            var serviceFaker = GetServiceFullGraphFaker(profileIds, categoryIds, medias);

            // 4. توليد الخدمات بالشكل البياني الكامل (Full Graph)
            for (int i = 0; i < countToGenerate; i++)
            {
                newServices.Add(serviceFaker.Generate());
            }

            // 5. الحفظ النهائي الذري في خطوة واحدة لجميع الجداول المرتبطة
            if (newServices.Any())
            {
                await db.Services.AddRangeAsync(newServices);
                await db.SaveChangesAsync();
            }

            return db.Services
                .Include(s => s.Category)
                .Include(s => s.ServiceProviderProfile)
                .Include(s => s.MediaGalleryLinks)
                    .ThenInclude(sm => sm.Media)
                .ToList();

            Console.WriteLine($"Successfully seeded {newServices.Count} services with their Categories and ServiceMedia links.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during complete Service & Category graph seeding: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            throw;
        }
    }


    [NonAction]
    public static Faker<Service> GetServiceFullGraphFaker(
    List<string> profileIds,
    List<int> categoryIds,
    List<Media> medias)
    {
        var mediaIds = medias.Select(m => m.Id).ToList();

        return new Faker<Service>("en")
            // 1. ربط العلاقات الخارجية الأساسية
            .RuleFor(s => s.ServiceProviderProfileId, f => f.PickRandom(profileIds))
            .RuleFor(s => s.CategoryId, f => f.PickRandom(categoryIds))

            // ربط صورة الغلاف الأساسية من الميديا المتاحة
            .RuleFor(s => s.MainMediaId, f => mediaIds.Any() ? f.PickRandom(mediaIds) : null)

            // 2. النصوص والبيانات الأساسية
            .RuleFor(s => s.Title, f => f.Commerce.ProductName().ClampLength(5, 80))
            .RuleFor(s => s.ShortDescription, f => f.Lorem.Sentence(10).ClampLength(10, 1000))
            .RuleFor(s => s.DetailedDescription, f => f.Lorem.Paragraphs(3).ClampLength(20, 4000))

            // 3. الأسعار والارقام القياسية
            .RuleFor(s => s.Price, f => Math.Round(f.Finance.Amount(5, 1000), 2))
            .RuleFor(s => s.DeliveryTimeInDays, f => f.Random.Number(1, 14))
            .RuleFor(s => s.RevisionCount, f => f.Random.Number(0, 10))

            // 4. مؤشرات الأداء والحالة
            .RuleFor(s => s.IsActive, true)
            .RuleFor(s => s.IsApproved, true)
            .RuleFor(s => s.TotalReviews, f => f.Random.Number(0, 150))
            .RuleFor(s => s.AverageRating, (f, s) => s.TotalReviews > 0 ? Math.Round(f.Random.Double(3.5, 5.0), 1) : 0)
            .RuleFor(s => s.SalesCount, f => f.Random.Number(0, 300))
            .RuleFor(s => s.ViewCount, (f, s) => s.SalesCount * f.Random.Number(5, 20)) // المشاهدات منطقياً أكبر من المبيعات

            // 5. مصفوفات النصوص والوسوم
            .RuleFor(s => s.Concepts, f => f.Make(3, () => f.Commerce.ProductAdjective()).ToList())

            .RuleFor(s => s.CreatedAt, f => f.Date.Past(1))

            // 6. حقن معرض الصور الفرعي (Media Gallery) مع ضمان عدم تكرار الصورة الأساسية فيه إن أردت
            .RuleFor(s => s.MediaGalleryLinks, f => {
                var serviceMediaLinks = new List<ServiceMedia>();

                if (mediaIds.Any())
                {
                    // اختيار من 1 إلى 3 صور عشوائية فريدة لمعرض الصور الخاص بالخدمة
                    var selectedMediaIds = f.PickRandom(mediaIds, f.Random.Number(1, 3)).Distinct().ToList();

                    foreach (var mediaId in selectedMediaIds)
                    {
                        serviceMediaLinks.Add(new ServiceMedia
                        {
                            MediaId = mediaId
                        });
                    }
                }

                return serviceMediaLinks;
            });
    }



    [NonAction]
    public async Task<List<ServiceOrder>> InjectServiceOrdersAsync(int countToGenerate = 40)
    {
        try
        {
            var faker = new Faker("en");

            // 1. جلب الخدمات المتاحة حالياً مع السعر ومعرف مقدم الخدمة المرتبط بها
            var services = await db.Services.ToListAsync();

            // 2. جلب معرفات جميع المستخدمين لتعيينهم كعملاء (Customers)
            var userIds = await db.Users.Select(u => u.Id).ToListAsync();

            // تحقق دفاعي لضمان وجود مراجع حقيقية في قاعدة البيانات
            if (!services.Any() || !userIds.Any())
            {
                Console.WriteLine("Warning: Seeding ServiceOrders skipped. Ensure Services and Users tables have data.");
                return new List<ServiceOrder>();
            }

            var serviceOrders = new List<ServiceOrder>();

            for (int i = 0; i < countToGenerate; i++)
            {
                // اختيار خدمة عشوائية من الكتالوج
                var selectedService = faker.PickRandom(services);

                // اختيار عميل عشوائي بشرط صارم: ألا يكون هو نفسه المستقل صاحب الخدمة
                var validCustomerIds = userIds.Where(id => id != selectedService.ServiceProviderProfileId).ToList();
                if (!validCustomerIds.Any()) continue;

                var customerId = faker.PickRandom(validCustomerIds);

                // تحديد حالة الطلب والتاريخ منطقياً
                var status = faker.PickRandom<OrderStatus>();
                var createdAt = faker.Date.Past(1);

                // تاريخ التسليم الفعلي يكون موجوداً فقط إذا كانت الحالة "Completed"
                DateTime? completionDate = (status == OrderStatus.Completed)
                    ? createdAt.AddDays(faker.Random.Number(1, 10))
                    : null;

                // تخزين السعر في متغير محلي لحل مشكلة الترتيب (CS0841)
                decimal servicePrice = selectedService.Price;

                // 3. بناء كيان المعاملة المالية المرتبط بالطلب
                var transaction = new PaymentTransaction
                {
                    Amount = servicePrice,
                    PlatformFee = servicePrice * 0.15m,
                    NetPayout = servicePrice - (servicePrice * 0.15m),
                    Currency = CurrencyCode.EGP,
                    Status = TransactionStatus.Completed,
                    GatewayUsed = faker.PickRandom<PaymentGateway>(),
                    TransactionDate = DateTime.UtcNow.AddMinutes(-30),
                    GatewayReferenceId = "PAY-" + faker.Random.AlphaNumeric(12).ToUpper()
                };

                

                // 4. [تعديل هـام للحل] إنشاء كائن محادثة فرعي مع تمرير الحقول الإلزامية في قاعدة البيانات
                var conversation = new Conversation
                {
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt,
                    CustomerId = customerId,                                      // معرف العميل
                    ProviderId = selectedService.ServiceProviderProfileId,         // معرف مقدم الخدمة (حل المشكلة الأساسية)
                    Title = $"محادثة طلب خدمة رقم #{i + 1}"                       // عنوان للمحادثة لمنع أي NullConstraint آخر
                };

                // 5. تجميع الـ Full Graph لطلب الخدمة
                var order = new ServiceOrder
                {
                    ServiceID = selectedService.Id,
                    CustomerId = customerId,
                    ServiceProviderId = selectedService.ServiceProviderProfileId,
                    AdditionalDetails = faker.Lorem.Sentence(12).ClampLength(0, 1000),
                    CompletionDate = completionDate,

                    Status = status,
                    Amount = servicePrice,
                    CreatedAt = createdAt,
                    UpdatedAt = status == OrderStatus.Completed ? completionDate : createdAt,

                    // حقن الكائنات التابعة مباشرة
                    PaymentTransaction = transaction,
                    Conversation = conversation
                };

                serviceOrders.Add(order);
            }

            // 6. الحفظ الذري الشامل في قاعدة البيانات
            if (serviceOrders.Any())
            {
                await db.ServiceOrders.AddRangeAsync(serviceOrders);
                await db.SaveChangesAsync();
                Console.WriteLine($"Successfully seeded {serviceOrders.Count} Service Orders with their dependent graphs.");
            }

            return db.ServiceOrders.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during ServiceOrder seeding: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            throw;
        }
    }



    [NonAction]
    public async Task<List<Dispute>> InjectDisputesAsync(List<int> serviceOrderIds, List<int> jobOrderIds)
    {
        try
        {
            var faker = new Faker("en");
            var newDisputes = new List<Dispute>();

            // جلب حساب آدمن عشوائي لتعيينه كمراجع للنزاعات المتقدمة (إذا تغيرت الحالة عن Opened)
            var adminRoleId = await db.Roles.Where(r => r.Name == RolesStrings.Admin).Select(r => r.Id).FirstOrDefaultAsync();
            //Error: you need to add Admin Useres
            var adminIds = await db.Users.Where(u => db.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == adminRoleId)).Select(u => u.Id).ToListAsync();

            // ==========================================
            // 1. معالجة طلبات الخدمات (Service Orders)
            // ==========================================
            if (serviceOrderIds != null && serviceOrderIds.Any())
            {
                // جلب الطلبات بكامل بياناتها لاستخراج الأطراف والمبالغ
                var serviceOrders = await db.ServiceOrders
                    .Where(o => serviceOrderIds.Contains(o.Id))
                    .ToListAsync();

                for (int i = 0; i < serviceOrders.Count; i++)
                {
                    
                    // تحقق دقيق: نسبة 1 من كل 20 طلب (بناءً على الاندكس)
                    if (i % 20 == 0)
                    {
                        var order = serviceOrders[i];

                        string? adminId = faker.PickRandom(adminIds);
                        string? RiaserId;
                        string? TargetId; ;

                        if (i % 2 == 0)
                        {
                            RiaserId = order.CustomerId;
                            TargetId = order.ServiceProviderId;
                        }
                        else
                        {
                            RiaserId = order.ServiceProviderId;
                            TargetId = order.CustomerId;
                        }
                       

                        // إنشاء المحادثات الإجبارية للنزاع أولاً لمنع كسر الـ Constraint
                        var raiserConv = new Conversation 
                        {
                            CreatedAt = DateTime.UtcNow,
                            Category = ConversationCategory.DisputeRaiser,
                            ContextType = ConversationContextType.Dispute,
                            ProviderId = adminId,
                            CustomerId = RiaserId,
                            Title = $"محادثة الرافع في نزاع طلب خدمة #{order.Id}"

                        };
                        var targetConv = new Conversation
                        {
                            CreatedAt = DateTime.UtcNow,
                            Category = ConversationCategory.DisputeTarget,
                            ContextType = ConversationContextType.Dispute,
                            ProviderId = adminId,
                            CustomerId = TargetId,
                            Title = $"محادثة المستهدف في نزاع طلب خدمة #{order.Id}"
                        };

                        await db.Conversations.AddRangeAsync(raiserConv, targetConv);
                        await db.SaveChangesAsync(); // للحصول على الـ IDs الخاصة بالمحادثات

                        var status = faker.PickRandom<DisputeStatus>();

                        var dispute = new Dispute
                        {
                            ServiceOrderId = order.Id,
                            JobOrderId = null, // هذا النزاع خاص بـ ServiceOrder

                            // افتراضياً: العميل هو من يرفع النزاع والمستقل هو المستهدف
                            RaiserId = order.CustomerId,
                            TargetId = order.ServiceProviderId,

                            AdminReviewerId = status != DisputeStatus.Opened ? adminId : null,

                            RaiserConversationId = raiserConv.Id,
                            TargetConversationId = targetConv.Id,

                            Status = status,
                            Type = faker.PickRandom<DisputeType>(),
                            AmountUnderDispute = order.Amount, // المبلغ المتنازع عليه هو قيمة الطلب
                            ReasonDetails = faker.Lorem.Sentence(15).ClampLength(10, 500),
                            OpenedDate = faker.Date.Past(1),

                            // محاكاة القرارات الإدارية إذا كان النزاع منتهياً
                            FinalDecisionDetails = status == DisputeStatus.Resolved || status == DisputeStatus.Closed
                                ? faker.Lorem.Sentence(10) : null,
                            IsDecisionAcceptedByRaiser = status == DisputeStatus.Resolved ? true : (bool?)null,
                            IsDecisionAcceptedByTarget = status == DisputeStatus.Resolved ? true : (bool?)null,
                            ResolutionDate = status == DisputeStatus.Resolved || status == DisputeStatus.Closed
                                ? DateTime.UtcNow : null
                        };

                        newDisputes.Add(dispute);
                    }
                }
            }

            // ==========================================
            // 2. معالجة طلبات المشاريع (Job Orders)
            // ==========================================
            if (jobOrderIds != null && jobOrderIds.Any())
            {
                var jobOrders = await db.JobOrders
                    .Where(o => jobOrderIds.Contains(o.Id))
                    .ToListAsync();

                for (int i = 0; i < jobOrders.Count; i++)
                {
                    // تحقق دقيق: نسبة 1 من كل 20 طلب
                    if (i % 20 == 0)
                    {
                        var order = jobOrders[i];


                        string? adminId = faker.PickRandom(adminIds);
                        string? RiaserId;
                        string? TargetId; ;

                        if (i % 2 == 0)
                        {
                            RiaserId = order.CustomerId;
                            TargetId = order.ServiceProviderId;
                        }
                        else
                        {
                            RiaserId = order.ServiceProviderId;
                            TargetId = order.CustomerId;
                        }


                        // إنشاء المحادثات الإجبارية للنزاع أولاً لمنع كسر الـ Constraint
                        var raiserConv = new Conversation
                        {
                            CreatedAt = DateTime.UtcNow,
                            Category = ConversationCategory.DisputeRaiser,
                            ContextType = ConversationContextType.Dispute,
                            ProviderId = adminId,
                            CustomerId = RiaserId,
                            Title = $"محادثة الرافع في نزاع طلب خدمة #{order.Id}"

                        };
                        var targetConv = new Conversation
                        {
                            CreatedAt = DateTime.UtcNow,
                            Category = ConversationCategory.DisputeTarget,
                            ContextType = ConversationContextType.Dispute,
                            ProviderId = adminId,
                            CustomerId = TargetId,
                            Title = $"محادثة المستهدف في نزاع طلب خدمة #{order.Id}"
                        };

                        await db.Conversations.AddRangeAsync(raiserConv, targetConv);
                        await db.SaveChangesAsync();

                        var status = faker.PickRandom<DisputeStatus>();

                        var dispute = new Dispute
                        {
                            ServiceOrderId = null,
                            JobOrderId = order.Id, // هذا النزاع خاص بـ JobOrder

                            RaiserId = order.CustomerId,
                            TargetId = order.ServiceProviderId,

                            AdminReviewerId = status != DisputeStatus.Opened ? faker.PickRandom(adminIds) : null,

                            RaiserConversationId = raiserConv.Id,
                            TargetConversationId = targetConv.Id,

                            Status = status,
                            Type = faker.PickRandom<DisputeType>(),
                            AmountUnderDispute = order.Amount, // قيمة الميزانية للمشروع
                            ReasonDetails = faker.Lorem.Sentence(15).ClampLength(10, 500),
                            OpenedDate = faker.Date.Past(1),

                            FinalDecisionDetails = status == DisputeStatus.Resolved || status == DisputeStatus.Closed
                                ? faker.Lorem.Sentence(10) : null,
                            IsDecisionAcceptedByRaiser = status == DisputeStatus.Resolved ? true : (bool?)null,
                            IsDecisionAcceptedByTarget = status == DisputeStatus.Resolved ? true : (bool?)null,
                            ResolutionDate = status == DisputeStatus.Resolved || status == DisputeStatus.Closed
                                ? DateTime.UtcNow : null
                        };
                        
                        newDisputes.Add(dispute);
                    }
                }
            }

            // ==========================================
            // 3. حفظ النزاعات المولّدة في قاعدة البيانات
            // ==========================================
            if (newDisputes.Any())
            {
                await db.Disputes.AddRangeAsync(newDisputes);
                await db.SaveChangesAsync();
                Console.WriteLine($"Successfully seeded {newDisputes.Count} disputes across orders.");
            }
            

            return db.Disputes.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during Dispute seeding: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            throw;
        }
    }

    [NonAction]
    public async Task<List<Review>> InjectReviewsAsync(int countToGenerate = 60)
    {
        try
        {
            var faker = new Faker("en");
            var reviews = new List<Review>();

            // 1. جلب طلبات الخدمات المكتملة لربطها بالتقييمات
            var completedServiceOrders = await db.ServiceOrders
                .Where(o => o.Status == OrderStatus.Completed && o.ReviewId == null)
                .Select(o => new { o.Id, o.CustomerId, o.ServiceProviderId, o.CreatedAt })
                .ToListAsync();

            // 2. جلب طلبات الأعمال الحرة المكتملة لربطها بالتقييمات
            var completedJobOrders = await db.JobOrders
                .Where(o => o.Status == OrderStatus.Completed && o.ReviewId == null)
                .Select(o => new { o.Id, o.CustomerId, o.ServiceProviderId, o.CreatedAt })
                .ToListAsync();

            // تحقق دفاعي: إذا لم يكن هناك طلبات مكتملة، نوقف التنفيذ لتجنب الأخطاء
            if (!completedServiceOrders.Any() && !completedJobOrders.Any())
            {
                Console.WriteLine("Warning: Seeding Reviews skipped. No completed ServiceOrders or JobOrders found without reviews.");
                return new List<Review>();
            }

            // عدادات لتتبع الموزع الفعلي
            int serviceIndex = 0;
            int jobIndex = 0;

            for (int i = 0; i < countToGenerate; i++)
            {
                Review review = new Review();

                // توزيع بالتناوب (50% لـ ServiceOrder و 50% لـ JobOrder) لضمان تغطية النوعين
                if (i % 2 == 0 && serviceIndex < completedServiceOrders.Count)
                {
                    var order = completedServiceOrders[serviceIndex++];

                    review.ServiceOrderId = order.Id;
                    review.JobOrderId = null; // حصرياً لطلب الخدمة
                    review.ReviewerId = order.CustomerId; // العميل الحقيقي للطلب
                    review.ServiceProviderId = order.ServiceProviderId; // المستقل الحقيقي للطلب
                    review.CreatedAt = order.CreatedAt.AddDays(faker.Random.Number(1, 5)); // تاريخ منطقي بعد الطلب
                }
                else if (jobIndex < completedJobOrders.Count)
                {
                    var order = completedJobOrders[jobIndex++];

                    review.JobOrderId = order.Id;
                    review.ServiceOrderId = null; // حصرياً لطلب العمل
                    review.ReviewerId = order.CustomerId;
                    review.ServiceProviderId = order.ServiceProviderId;
                    review.CreatedAt = order.CreatedAt.AddDays(faker.Random.Number(1, 5));
                }
                else if (serviceIndex < completedServiceOrders.Count) // Fallback إذا انتهت طلبات العمل الحر
                {
                    var order = completedServiceOrders[serviceIndex++];
                    review.ServiceOrderId = order.Id;
                    review.JobOrderId = null;
                    review.ReviewerId = order.CustomerId;
                    review.ServiceProviderId = order.ServiceProviderId;
                    review.CreatedAt = order.CreatedAt.AddDays(faker.Random.Number(1, 5));
                }
                else
                {
                    break; // لا يوجد المزيد من الطلبات المكتملة وغير المقيّمة
                }

                // توليد النصوص والتقييم الرقمي بحسب متطلبات الـ Data Annotations في الكلاس
                review.Rating = faker.Random.Double(1, 5);

                review.Title = faker.PickRandom(new[] { "Excellent Work", "Very Professional", "Great Communication", "Good Quality", "Fast Delivery" });
                review.Content = faker.Lorem.Paragraph().ClampLength(2, 500); // الالتزام بـ StringLength(500)
                review.UpdatedAt = review.CreatedAt;

                reviews.Add(review);
            }

            // 3. حفظ التقييمات وتحديث الطلبات لربط الـ ReviewId العكسي
            if (reviews.Any())
            {
                await db.Reviews.AddRangeAsync(reviews);
                await db.SaveChangesAsync(); // حفظ أولاً لتوليد معرفات الـ Review Ids

                // 4. تحديث الـ Foreign Keys العكسية في جداول الطلبات (للحفاظ على الـ 1:1 Navigation إذا وُجدت)
                foreach (var r in reviews)
                {
                    if (r.ServiceOrderId.HasValue)
                    {
                        var boundOrder = await db.ServiceOrders.FindAsync(r.ServiceOrderId.Value);
                        if (boundOrder != null) boundOrder.ReviewId = r.Id;
                    }
                    else if (r.JobOrderId.HasValue)
                    {
                        var boundOrder = await db.JobOrders.FindAsync(r.JobOrderId.Value);
                        if (boundOrder != null) boundOrder.ReviewId = r.Id;
                    }
                }

                await db.SaveChangesAsync(); // حفظ التحديث العكسي للطلبات
                Console.WriteLine($"Successfully seeded {reviews.Count} Reviews for both Service and Job orders.");
                
            }

            return db.Reviews.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during Reviews seeding: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            throw;
        }
    }

    //END OF MY WORK:::

    private async Task<List<Media>> GetOrCreateSeedMediaAsync(Database db, IWebHostEnvironment env)
    {



        var existing = await db.Medias
            .Where(m => m.FileName.StartsWith(SeedMediaPrefix))
            .OrderBy(m => m.FileName)
            .Take(20)
            .ToListAsync();

        if (existing.Count >= 20)
            return existing;

        var uploadsPath = Path.Combine(env.WebRootPath, "Uploads");
        if (!Directory.Exists(uploadsPath))
            Directory.CreateDirectory(uploadsPath);

        var colors = new[] { "FF5733", "33FF57", "3357FF", "FF33F5", "F5FF33", "33FFF5", "FF8C33", "8C33FF", "33FF8C", "FF3333" };

        for (var i = 1; i <= 20; i++)
        {
            var fileName = $"{SeedMediaPrefix}{i:D2}.png";
            if (await db.Medias.AnyAsync(m => m.FileName == fileName))
                continue;

            var filePath = Path.Combine(uploadsPath, fileName);
            await CreateSimpleImageAsync(filePath, colors[i % colors.Length]);

            db.Medias.Add(new Media
            {
                FileName = fileName,
                ContentType = "image/png",
                FileExtension = ".png",
                Size = new FileInfo(filePath).Length
            });
        }

        await db.SaveChangesAsync();

        return await db.Medias
            .Where(m => m.FileName.StartsWith(SeedMediaPrefix))
            .OrderBy(m => m.FileName)
            .Take(20)
            .ToListAsync();
    }

    private async Task CreateSimpleImageAsync(string filePath, string hexColor)
    {
        // إنشاء صورة PNG بسيطة 400x400
        var width = 400;
        var height = 400;

        // تحويل HEX إلى RGB
        var r = Convert.ToByte(hexColor.Substring(0, 2), 16);
        var g = Convert.ToByte(hexColor.Substring(2, 2), 16);
        var b = Convert.ToByte(hexColor.Substring(4, 2), 16);

        // إنشاء PNG بسيط
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            // PNG Header
            await stream.WriteAsync(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A });

            // IHDR chunk
            var ihdr = new List<byte>();
            ihdr.AddRange(BitConverter.GetBytes(width).Reverse());
            ihdr.AddRange(BitConverter.GetBytes(height).Reverse());
            ihdr.AddRange(new byte[] { 8, 2, 0, 0, 0 }); // bit depth, color type, compression, filter, interlace

            await WriteChunkAsync(stream, "IHDR", ihdr.ToArray());

            // IDAT chunk (simplified - solid color)
            var idat = new List<byte>();
            for (int y = 0; y < height; y++)
            {
                idat.Add(0); // filter type
                for (int x = 0; x < width; x++)
                {
                    idat.AddRange(new byte[] { r, g, b });
                }
            }

            await WriteChunkAsync(stream, "IDAT", CompressData(idat.ToArray()));

            // IEND chunk
            await WriteChunkAsync(stream, "IEND", Array.Empty<byte>());
        }
    }

    private async Task WriteChunkAsync(FileStream stream, string type, byte[] data)
    {
        var length = BitConverter.GetBytes(data.Length).Reverse().ToArray();
        await stream.WriteAsync(length);
        
        var typeBytes = Encoding.ASCII.GetBytes(type);
        await stream.WriteAsync(typeBytes);
        await stream.WriteAsync(data);

        // CRC
        var crc = CalculateCRC(typeBytes.Concat(data).ToArray());
        await stream.WriteAsync(BitConverter.GetBytes(crc).Reverse().ToArray());
    }

    private byte[] CompressData(byte[] data)
    {
        using (var output = new MemoryStream())
        {
            using (var deflate = new System.IO.Compression.DeflateStream(output, System.IO.Compression.CompressionMode.Compress))
            {
                deflate.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }
    }

    private uint CalculateCRC(byte[] data)
    {
        uint crc = 0xFFFFFFFF;
        foreach (var b in data)
        {
            crc ^= b;
            for (int i = 0; i < 8; i++)
            {
                crc = (crc & 1) != 0 ? (crc >> 1) ^ 0xEDB88320 : crc >> 1;
            }
        }
        return ~crc;
    }

    

    private async Task<List<Skill>> GetOrCreateSeedSkillsAsync(Database db)
    {
        var names = new[]
        {
            "C#", "ASP.NET Core", "React", "Angular", "Vue.js", "Python", "Django", "Node.js",
            "Photoshop", "Illustrator", "Figma", "UI/UX Design", "Content Writing", "SEO", "Social Media Marketing"
        };

        var list = new List<Skill>();
        foreach (var name in names)
        {
            var s = await db.Skills.FirstOrDefaultAsync(x => x.Name == name);
            if (s == null)
            {
                s = new Skill { Name = name };
                db.Skills.Add(s);
                await db.SaveChangesAsync();
            }

            list.Add(s);
        }

        return list;
    }

    private async Task<List<User>> GetOrCreateSeedUsersAsync(UserManager<User> userManager, List<Media> mediaList)
    {
        const string password = "Giggo343@";
        var userNames = new[]
        {
            "أحمد محمد", "فاطمة علي", "محمد حسن", "نور الدين", "سارة أحمد",
            "عمر خالد", "ليلى محمود", "يوسف إبراهيم", "مريم سعيد", "كريم عبدالله"
        };

        var users = new List<User>();
        var createErrors = new List<string>();

        for (var i = 0; i < 10; i++)
        {
            var userName = $"seed_user_{i + 1}";
            var existing = await userManager.FindByNameAsync(userName);
            if (existing != null)
            {
                users.Add(existing);
                continue;
            }

            var user = new User
            {
                FullName = userNames[i],
                Email = $"{userName}{SeedEmailDomain}",
                UserName = userName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Amount = Random.Shared.Next(100, 10000),
                DateOfBirth = DateTime.UtcNow.AddYears(-Random.Shared.Next(20, 50)),
                Role = i < 5 ? "Freelancer" : "Client",
                Status = "Active",
                IsTrustedByAdmin = i < 3,
                ProfilePictureId = mediaList[i % mediaList.Count].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 365))
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
                users.Add(user);
            else
                createErrors.Add($"{userName}: {string.Join("; ", result.Errors.Select(e => $"{e.Code}:{e.Description}"))}");
        }

        if (users.Count < 10)
        {
            throw new InvalidOperationException(
                $"Expected 10 seed users, have {users.Count}. Identity errors: {string.Join(" | ", createErrors)}");
        }

        return users.OrderBy(u => int.Parse(u.UserName!.Replace("seed_user_", ""), CultureInfo.InvariantCulture)).ToList();
    }

    private async Task<List<ServiceProviderProfile>> GetOrCreateSeedProviderProfilesAsync(
        Database db,
        List<User> users,
        List<Media> mediaList,
        List<Skill> skills)
    {
        if (users.Count < 10 || mediaList.Count < 20 || skills.Count < 10)
            throw new InvalidOperationException("Insufficient seed users, media, or skills for provider profiles.");

        var freelancers = users.Take(5).ToList();
        var profiles = new List<ServiceProviderProfile>();

        var jobTitles = new[] { "مطور Full Stack", "مصمم جرافيك", "كاتب محتوى", "مسوق رقمي", "مطور تطبيقات" };
        var bios = new[]
        {
            "مطور محترف مع خبرة 5 سنوات في تطوير تطبيقات الويب",
            "مصمم مبدع متخصص في تصميم الهويات البصرية",
            "كاتب محتوى إبداعي مع خبرة في كتابة المقالات التقنية",
            "خبير تسويق رقمي متخصص في إدارة حملات السوشيال ميديا",
            "مطور تطبيقات موبايل محترف"
        };

        for (var i = 0; i < 5; i++)
        {
            var uid = freelancers[i].Id;
            var existing = await db.ServiceProviderProfiles.FirstOrDefaultAsync(p => p.UserId == uid);
            if (existing != null)
            {
                profiles.Add(existing);
                continue;
            }

            var profile = new ServiceProviderProfile
            {
                UserId = uid,
                JobTitle = jobTitles[i],
                Bio = bios[i],
                ExperienceYears = Random.Shared.Next(1, 10),
                HourlyRate = Random.Shared.Next(50, 500),
                WorkingHoursPerWeek = Random.Shared.Next(20, 40),
                AverageRating = Random.Shared.Next(35, 50) / 10.0,
                TotalReviews = Random.Shared.Next(10, 100),
                CompletedJobs = Random.Shared.Next(5, 50),
                IsActive = true,
                IsAvailable = true,
                DateOfJoin = DateTime.UtcNow.AddDays(-Random.Shared.Next(30, 365)),
                Skills =
                [
                    new ProviderSkill
                    {
                        SkillId = skills[(i * 2) % skills.Count].Id,
                        MyLevel = (SkillExperienceLevel)Random.Shared.Next(1, 6)
                    },
                    new ProviderSkill
                    {
                        SkillId = skills[(i * 2 + 1) % skills.Count].Id,
                        MyLevel = (SkillExperienceLevel)Random.Shared.Next(1, 6)
                    }
                ],
                Certificates =
                [
                    new Certificate
                    {
                        Title = "شهادة احترافية",
                        Issuer = "منصة تعليمية",
                        Type = "Professional",
                        YearAcquired = DateTime.UtcNow.Year - Random.Shared.Next(1, 5),
                        MediaId = mediaList[(i + 10) % mediaList.Count].Id
                    }
                ],
                PortfolioItems =
                [
                    new PortfolioItem
                    {
                        Title = $"{SeedMarker} مشروع {i + 1}",
                        Description = "وصف المشروع السابق",
                        ProjectUrl = "https://example.com",
                        CompletionDate = DateTime.UtcNow.AddDays(-Random.Shared.Next(30, 365)),
                        ProjectMediaLinks =
                        [
                            new PortfolioMedia { MediaId = mediaList[(i + 15) % mediaList.Count].Id }
                        ]
                    }
                ]
            };

            db.ServiceProviderProfiles.Add(profile);
            await db.SaveChangesAsync();
            profiles.Add(profile);
        }

        return profiles;
    }

    private async Task<List<Service>> GetOrCreateSeedServicesAsync(
        Database db,
        List<ServiceProviderProfile> providers,
        List<Category> categories,
        List<Media> mediaList)
    {
        var baseTitles = new[]
        {
            "تطوير موقع ويب متكامل", "تصميم شعار احترافي", "كتابة مقال SEO",
            "إدارة حسابات السوشيال ميديا", "تطوير تطبيق موبايل", "تصميم بنر إعلاني",
            "ترجمة محتوى", "مونتاج فيديو", "استشارة تسويقية", "تصميم واجهة مستخدم"
        };

        var seededTitles = baseTitles.Select(t => $"{SeedMarker}|{t}").ToList();
        var existing = await db.Services.Where(s => seededTitles.Contains(s.Title)).ToListAsync();
        if (existing.Count >= 10)
            return existing.OrderBy(s => s.Id).Take(10).ToList();

        for (var i = 0; i < 10; i++)
        {
            var title = seededTitles[i];
            if (await db.Services.AnyAsync(s => s.Title == title))
                continue;

            db.Services.Add(new Service
            {
                Title = title,
                ShortDescription = $"وصف مختصر للخدمة {i + 1}",
                DetailedDescription = $"وصف تفصيلي للخدمة {i + 1} مع شرح كامل للمميزات والفوائد",
                Price = Random.Shared.Next(50, 1000),
                DeliveryTimeInDays = Random.Shared.Next(1, 30),
                RevisionCount = Random.Shared.Next(1, 5),
                AverageRating = Random.Shared.Next(35, 50) / 10.0,
                TotalReviews = Random.Shared.Next(5, 50),
                CategoryId = categories[i % categories.Count].Id,
                ServiceProviderProfileId = providers[i % providers.Count].UserId,
                MainMediaId = mediaList[i % mediaList.Count].Id,
                Concepts = new List<string> { "مفهوم 1", "مفهوم 2", "مفهوم 3" },
                MediaGalleryLinks =
                [
                    new ServiceMedia { MediaId = mediaList[(i + 1) % mediaList.Count].Id },
                    new ServiceMedia { MediaId = mediaList[(i + 2) % mediaList.Count].Id }
                ]
            });
        }

        await db.SaveChangesAsync();

        return await db.Services.Where(s => seededTitles.Contains(s.Title)).OrderBy(s => s.Id).Take(10).ToListAsync();
    }

    private async Task<List<JobPost>> GetOrCreateSeedJobPostsAsync(Database db, List<User> users, List<Category> categories)
    {
        if (users.Count < 10)
            throw new InvalidOperationException($"Not enough users. Expected at least 10, got {users.Count}");

        var clients = users.Skip(5).Take(5).ToList();
        var baseTitles = new[]
        {
            "مطلوب مطور ويب", "مطلوب مصمم جرافيك", "مطلوب كاتب محتوى",
            "مطلوب مسوق رقمي", "مطلوب مطور تطبيقات"
        };

        var seededTitles = baseTitles.Select(t => $"{SeedMarker}|{t}").ToList();
        var existing = await db.JobPosts.Where(j => seededTitles.Contains(j.Title)).ToListAsync();
        if (existing.Count >= 5)
            return existing.OrderBy(j => j.Id).Take(5).ToList();

        for (var i = 0; i < 5; i++)
        {
            var title = seededTitles[i];
            if (await db.JobPosts.AnyAsync(j => j.Title == title))
                continue;

            db.JobPosts.Add(new JobPost
            {
                Title = title,
                Description = $"وصف تفصيلي للوظيفة {i + 1}",
                BudgetMin = Random.Shared.Next(500, 2000),
                BudgetMax = Random.Shared.Next(2000, 5000),
                Deadline = DateTime.UtcNow.AddDays(Random.Shared.Next(7, 30)),
                Status = JobPostStatus.Open,
                CustomerId = clients[i].Id,
                CategoryId = categories[i % categories.Count].Id,
                ExperienceLevel = (ExperienceLevel)Random.Shared.Next(1, 4),
                ProjectLength = Random.Shared.Next(1, 6) + " months",
                TimeCommitment = (TimeCommit)Random.Shared.Next(0, 2),
                Offers = new List<JobOffer>
                {
                    new JobOffer
                    {
                        ProviderProfileId = users[i].Id,
                        Amount = Random.Shared.Next(500, 5000),
                        Description = $"خطاب تغطية للعرض {i + 1}",
                        Status = JobOfferStatus.Pending
                    },
                    new JobOffer
                    {
                        ProviderProfileId = users[i].Id,
                        Amount = Random.Shared.Next(500, 5000),
                        Description = $"خطاب تغطية للعرض {i + 2}",
                        Status = JobOfferStatus.Pending
                    },
                },
                
            });
        }

        await db.SaveChangesAsync();

        return await db.JobPosts.Where(j => seededTitles.Contains(j.Title)).OrderBy(j => j.Id).Take(5).ToListAsync();
    }

    private async Task GetOrCreateSeedJobSkillRequirementsAsync(Database db, List<JobPost> jobPosts, List<Skill> skills)
    {
        for (var idx = 0; idx < jobPosts.Count; idx++)
        {
            var jp = jobPosts[idx];
            if (await db.JobSkillRequirements.AnyAsync(r => r.JobPostId == jp.Id))
                continue;

            db.JobSkillRequirements.Add(new JobSkillRequirement
            {
                JobPostId = jp.Id,
                SkillId = skills[idx * 2 % skills.Count].Id,
                RequiredLevel = (SkillExperienceLevel)Random.Shared.Next(2, 5)
            });
        }

        await db.SaveChangesAsync();
    }

    private async Task<List<ServiceOrder>> GetOrCreateSeedServiceOrdersAsync(
        Database db,
        List<Service> services,
        List<User> users,
        List<ServiceProviderProfile> providers)
    {
        if (services.Count < 5 || users.Count < 10 || providers.Count < 5)
            throw new InvalidOperationException("Insufficient data for service orders.");

        var existing = await db.ServiceOrders
            .Where(o => o.AdditionalDetails != null && o.AdditionalDetails.StartsWith(SeedOrderMarker))
            .OrderBy(o => o.Id)
            .Take(5)
            .ToListAsync();

        if (existing.Count >= 5)
            return existing;

        var payments = new List<PaymentTransaction>();
        var conversations = new List<Conversation>();

        for (var i = 0; i < 5; i++)
        {
            var service = services[i];
            var customer = users[5 + i];
            var providerUserId = providers[i % providers.Count].UserId;
            var amount = service.Price;
            var platformFee = Math.Round(amount * 0.1m, 2, MidpointRounding.AwayFromZero);
            if (platformFee <= 0 || platformFee >= amount)
                platformFee = Math.Max(0.01m, Math.Round(amount * 0.05m, 2, MidpointRounding.AwayFromZero));
            var netPayout = amount - platformFee;
            if (netPayout <= 0)
                netPayout = Math.Max(0.01m, amount * 0.9m);

            payments.Add(new PaymentTransaction
            {
                Amount = amount,
                PlatformFee = platformFee,
                NetPayout = netPayout,
                Currency = CurrencyCode.EGP,
                Status = TransactionStatus.Completed,
                GatewayUsed = PaymentGateway.Card
            });

            conversations.Add(new Conversation
            {
                RelatedEntityId = service.Id,
                Title = $"محادثة طلب: {service.Title}",
                CustomerId = customer.Id,
                ProviderId = providerUserId,
                Category = ConversationCategory.Standard,
                ContextType = ConversationContextType.ServiceOrder,
                CreatedBy = SeedCreatedBy
            });
        }

        await db.PaymentTransactions.AddRangeAsync(payments);
        await db.Conversations.AddRangeAsync(conversations);
        await db.SaveChangesAsync();

        for (var i = 0; i < 5; i++)
        {
            var service = services[i];
            db.ServiceOrders.Add(new ServiceOrder
            {
                ServiceID = service.Id,
                CustomerId = users[5 + i].Id,
                ServiceProviderId = providers[i % providers.Count].UserId,
                Amount = service.Price,
                Status = (OrderStatus)Random.Shared.Next(0, 7),
                AdditionalDetails = $"{SeedOrderMarker}تفاصيل إضافية للطلب {i + 1}",
                CompletionDate = DateTime.UtcNow.AddDays(Random.Shared.Next(-30, 30)),
                PaymentTransactionId = payments[i].Id,
                ConversationId = conversations[i].Id,
                CreatedBy = SeedCreatedBy
            });
        }

        await db.SaveChangesAsync();

        return await db.ServiceOrders
            .Where(o => o.AdditionalDetails != null && o.AdditionalDetails.StartsWith(SeedOrderMarker))
            .OrderBy(o => o.Id)
            .Take(5)
            .ToListAsync();
    }

    private async Task GetOrCreateSeedReviewsAsync(
        Database db,
        List<ServiceOrder> orders,
        List<ServiceProviderProfile> providers)
    {
        for (var i = 0; i < orders.Count; i++)
        {
            var title = $"{SeedMarker}|Review {orders[i].Id}";
            if (await db.Reviews.AnyAsync(r => r.ServiceOrderId == orders[i].Id && r.Title.StartsWith(SeedMarker)))
                continue;

            db.Reviews.Add(new Review
            {
                Title = title,
                Content = $"محتوى التقييم {i + 1} - خدمة ممتازة وسريعة",
                Rating = Random.Shared.Next(3, 6),
                ServiceOrderId = orders[i].Id,
                ReviewerId = orders[i].CustomerId,
                ServiceProviderId = providers[i % providers.Count].UserId,
                CreatedBy = SeedCreatedBy
            });
        }

        await db.SaveChangesAsync();
    }

    private async Task EnsureSeedExtensionEntitiesAsync(
        Database db,
        List<User> users,
        List<Service> services,
        List<ServiceOrder> serviceOrders,
        List<JobPost> jobPosts,
        List<Media> mediaList)
    {
        foreach (var jp in jobPosts)
        {
            await db.Entry(jp).Collection(x => x.MileStones).LoadAsync();
            if (jp.MileStones.Count != 0)
                continue;

            jp.MileStones.Add(new MileStone
            {
                Title = $"{SeedMarker} مرحلة أولى",
                Description = "مرحلة تسليم أولية للوظيفة المزروعة.",
                StepNumber = 1,
                IsCompleted = false,
                Price = 500
            });
            jp.MileStones.Add(new MileStone
            {
                Title = $"{SeedMarker} مرحلة ثانية",
                Description = "مرحلة تسليم ثانية للوظيفة المزروعة.",
                StepNumber = 2,
                IsCompleted = false,
                Price = 800
            });
        }

        await db.SaveChangesAsync();

        var jp0 = await db.JobPosts
            .Include(j => j.MileStones)
            .Include(j => j.DeliveredFiles)
            .FirstOrDefaultAsync(j => jobPosts.Select(p => p.Id).Contains(j.Id));

        if (jp0 is { MileStones.Count: > 0 } && jp0.DeliveredFiles.Count == 0)
        {
            var ms = jp0.MileStones.OrderBy(m => m.StepNumber).First();
            db.Set<DeliveredJobFile>().Add(new DeliveredJobFile
            {
                JobId = jp0.Id,
                MileStoneId = ms.Id,
                MediaId = mediaList[0].Id,
                Statues = DeliveredFileStatues.New
            });
            await db.SaveChangesAsync();
        }

        var so0 = serviceOrders.FirstOrDefault();
        if (so0 != null)
        {
            var conv = await db.Conversations.Include(c => c.Messages).FirstOrDefaultAsync(c => c.Id == so0.ConversationId);
            if (conv != null && !conv.Messages.Any(m => m.Content == $"{SeedMarker}|hello"))
            {
                conv.Messages.Add(new Message
                {
                    SenderId = so0.CustomerId,
                    Content = $"{SeedMarker}|hello",
                    IsRead = false,
                    CreatedBy = SeedCreatedBy
                });
                await db.SaveChangesAsync();
            }
        }

        var favUser = users[5].Id;
        var favSvc = services[0].Id;
        if (!await db.UserFavorites.AnyAsync(f => f.UserID == favUser && f.ServiceID == favSvc))
        {
            db.UserFavorites.Add(new UserFavorites { UserID = favUser, ServiceID = favSvc });
            await db.SaveChangesAsync();
        }

        var u0 = users[0].Id;
        if (!await db.CreditCards.AnyAsync(c => c.UserId == u0 && c.Last4Digits == "4242"))
        {
            db.CreditCards.Add(new CreditCard
            {
                UserId = u0,
                Tokenized = $"{SeedMarker}-tok",
                Last4Digits = "4242",
                ExpirationDate = DateTime.UtcNow.AddYears(2),
                CardType = "Visa"
            });
            await db.SaveChangesAsync();
        }

        if (!await db.RefreshTokens.AnyAsync(t => t.Token == $"{SeedMarker}_refresh"))
        {
            db.RefreshTokens.Add(new RefreshTokens
            {
                UserId = u0,
                Token = $"{SeedMarker}_refresh",
                ExpireAt = DateTime.UtcNow.AddDays(30),
                CreatedAt = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }

        if (!await db.VerificationData.AnyAsync(v => v.UserId == u0))
        {
            db.VerificationData.Add(new VerificationData
            {
                UserId = u0,
                NationalNumber = "SEED12345678901",
                Country = "EG",
                City = "Cairo",
                Status = VerificationStatus.Approved
            });
            await db.SaveChangesAsync();
        }

        if (!await db.VerificationsCodes.AnyAsync(v => v.UserId == u0 && v.Type == VerificationCodeType.changePassword))
        {
            db.VerificationsCodes.Add(new VerificationsCodes
            {
                UserId = u0,
                Type = VerificationCodeType.changePassword,
                Value = Random.Shared.Next(VerificationsCodesConstrains.MinValue, VerificationsCodesConstrains.MaxValue + 1),
                CreatedBy = SeedCreatedBy
            });
            await db.SaveChangesAsync();
        }

        if (!await db.Reports.AnyAsync(r => r.JobId.Contains(SeedMarker)))
        {
            var rep = new Report
            {
                JobId = $"{SeedMarker}|post-{jobPosts[0].Id}",
                ClientName = users[5].FullName,
                FreelancerName = users[0].FullName,
                Type = "Quality",
                Reason = SeedMarker,
                Description = "Seed report body.",
                Status = "Pending"
            };
            rep.Messages.Add(new ReportMessage { SenderName = "admin", Text = $"{SeedMarker} follow-up", IsAdmin = true });
            rep.Attachments.Add(new ReportAttachment
            {
                FileName = "seed.txt",
                Url = "https://example.com/seed.txt",
                Type = "doc"
            });
            db.Reports.Add(rep);
            await db.SaveChangesAsync();
        }

        if (!await db.Set<Job>().AnyAsync(j => j.Title.StartsWith(SeedMarker)))
        {
            db.Set<Job>().Add(new Job
            {
                Title = $"{SeedMarker} Legacy job listing",
                Description = "Seed row for Identity Job table (mapped via User).",
                Budget = 500,
                Status = "Open",
                UserId = users[5].Id
            });
            await db.SaveChangesAsync();
        }

        if (serviceOrders.Count > 0 &&
            !await db.Disputes.AnyAsync(d => d.ReasonDetails != null && d.ReasonDetails.Contains("SEED_DISPUTE")))
        {
            var order = serviceOrders[0];
            var raiser = order.CustomerId;
            var target = order.ServiceProviderId;
            var raiserConv = new Conversation
            {
                RelatedEntityId = order.Id,
                Title = $"{SeedMarker} dispute-raiser",
                CustomerId = raiser,
                ProviderId = target,
                Category = ConversationCategory.DisputeRaiser,
                ContextType = ConversationContextType.Dispute,
                CreatedBy = SeedCreatedBy
            };
            var targetConv = new Conversation
            {
                RelatedEntityId = order.Id,
                Title = $"{SeedMarker} dispute-target",
                CustomerId = target,
                ProviderId = raiser,
                Category = ConversationCategory.DisputeTarget,
                ContextType = ConversationContextType.Dispute,
                CreatedBy = SeedCreatedBy
            };
            db.Conversations.AddRange(raiserConv, targetConv);
            await db.SaveChangesAsync();

            db.Disputes.Add(new Dispute
            {
                ServiceOrderId = order.Id,
                RaiserId = raiser,
                TargetId = target,
                RaiserConversationId = raiserConv.Id,
                TargetConversationId = targetConv.Id,
                Type = DisputeType.Other,
                AmountUnderDispute = order.Amount,
                Status = DisputeStatus.Opened,
                ReasonDetails = "SEED_DISPUTE",
                CreatedBy = SeedCreatedBy,
                OpenedDate = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }
    }

    private async Task<List<JobOrder>> GetOrCreateSeedJobOrdersAsync(Database db, List<JobPost> jobPosts)
    {
        var seededJobOrders = new List<JobOrder>();
        var postsWithOptions = await db.JobPosts.Include(jp => jp.Offers).Where(jp => jobPosts.Select(p => p.Id).Contains(jp.Id)).ToListAsync();

        foreach (var jp in postsWithOptions)
        {
            if (jp.Offers.Count > 0)
            {
                var offer = jp.Offers.First();

                if (!await db.JobOrders.AnyAsync(jo => jo.JobPostId == jp.Id && jo.AcceptedOfferId == offer.Id))
                {
                    // Create PaymentTransaction first to satisfy relation
                    var payment = new PaymentTransaction
                    {
                        Amount = offer.Amount,
                        PlatformFee = offer.Amount * 0.1m,
                        NetPayout = offer.Amount * 0.9m,
                        Currency = CurrencyCode.EGP,
                        Status = TransactionStatus.Completed,
                        GatewayUsed = PaymentGateway.Card
                    };
                    db.PaymentTransactions.Add(payment);
                    await db.SaveChangesAsync();

                    var order = JobOrder.BuildOrder(jp, offer);
                    order.PaymentTransactionId = payment.Id;
                    order.ExpectedDeliveryDate = DateTime.UtcNow.AddDays(offer.DeliveryTimeInDays == 0 ? 5 : offer.DeliveryTimeInDays);
                    order.CreatedBy = SeedCreatedBy;
                    if (order.Conversation != null)
                    {
                        order.Conversation.CreatedBy = SeedCreatedBy;
                    }
                    db.JobOrders.Add(order);
                }
            }
        }
        await db.SaveChangesAsync();

        return await db.JobOrders.Where(jo => jo.CreatedBy == SeedCreatedBy).ToListAsync();
    }

    private async Task<List<JobDeliverable>> GetOrCreateSeedJobDeliverablesAsync(Database db, List<JobOrder> jobOrders, List<Media> mediaList)
    {
        foreach (var order in jobOrders)
        {
            if (!await db.JobDeliverables.AnyAsync(jd => jd.JobOrderId == order.Id))
            {
                var deliverable = new JobDeliverable
                {
                    JobOrderId = order.Id,
                    Description = $"{SeedMarker} - تسليم عمل للطلب {order.Id}",
                    Attachments = new List<Media>()
                };

                if (mediaList.Count > 0)
                {
                    deliverable.Attachments.Add(mediaList[Random.Shared.Next(0, mediaList.Count)]);
                }

                db.JobDeliverables.Add(deliverable);
            }
        }
        await db.SaveChangesAsync();

        return await db.JobDeliverables.Where(jd => jd.Description.StartsWith(SeedMarker)).ToListAsync();
    }

    #endregion
}