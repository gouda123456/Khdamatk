using Khdamatk.Server.Data;
using Khdamatk.Server.Data.Entities.Catalog;
using Khdamatk.Server.Data.Entities.Identity;
using Khdamatk.Server.Data.Entities.Financial;
using Khdamatk.Server.Data.Entities.Interaction;
using Khdamatk.Server.Data.Entities.Operations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;

namespace Khdamatk.Server.Services.Implementations;

/// <summary>
/// خدمة لملء قاعدة البيانات ببيانات تجريبية شاملة
/// تغطي جميع الـ 35 جدول في المشروع
/// </summary>
public class DataSeederService
{
    private readonly Database _db;
    private readonly UserManager<User> _userManager;
    private readonly string _uploadsPath;
    private readonly Random _random = new();

    public DataSeederService(Database db, UserManager<User> userManager, IWebHostEnvironment env)
    {
        _db = db;
        _userManager = userManager;
        _uploadsPath = Path.Combine(env.WebRootPath, "uploads");
        
        if (!Directory.Exists(_uploadsPath))
            Directory.CreateDirectory(_uploadsPath);
    }

    /// <summary>
    /// تشغيل عملية ملء البيانات الشاملة
    /// </summary>
    public async Task<bool> SeedAllDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. التحقق من وجود بيانات موجودة
            if (await _db.Users.CountAsync(cancellationToken) > 0)
                return false; // البيانات موجودة بالفعل

            // 2. إنشاء الملفات الصور
            var mediaFiles = await CreateMediaFilesAsync(cancellationToken);

            // 3. إنشاء المستخدمين والأدوار
            var users = await CreateUsersAndRolesAsync(mediaFiles, cancellationToken);

            // 4. التصنيفات والمهارات
            var categories = await CreateCategoriesAsync(cancellationToken);
            var skills = await CreateSkillsAsync(cancellationToken);

            // 5. ملفات مقدمي الخدمة
            var serviceProviders = await CreateServiceProvidersAsync(users, mediaFiles, skills, cancellationToken);

            // 6. الخدمات والخدمات المرتبطة
            var services = await CreateServicesAsync(serviceProviders, categories, mediaFiles, cancellationToken);

            // 7. الشهادات والأعمال السابقة
            await CreateCertificatesAndPortfolioAsync(serviceProviders, mediaFiles, cancellationToken);

            // 8. إعلانات الوظائف والعروض
            var jobPosts = await CreateJobPostsAsync(users, categories, skills, mediaFiles, cancellationToken);
            var jobOffers = await CreateJobOffersAsync(jobPosts, serviceProviders, mediaFiles, cancellationToken);

            // 9. طلبات الخدمات والوظائف
            var serviceOrders = await CreateServiceOrdersAsync(users, services, cancellationToken);
            var jobOrders = await CreateJobOrdersAsync(users, jobPosts, serviceProviders, jobOffers, cancellationToken);

            // 10. المعاملات المالية
            await CreatePaymentTransactionsAsync(serviceOrders, jobOrders, cancellationToken);

            // 11. البطاقات الائتمانية
            await CreateCreditCardsAsync(users, cancellationToken);

            // 12. المحادثات والرسائل
            await CreateConversationsAndMessagesAsync(serviceOrders, jobOrders, users, cancellationToken);

            // 13. التقييمات والنزاعات
            await CreateReviewsAndDisputesAsync(serviceOrders, jobOrders, users, cancellationToken);

            // 14. المفضلة والتقارير
            await CreateUserFavoritesAndReportsAsync(users, services, cancellationToken);

            // 15. المسلمات والملفات المسلمة
            await CreateDeliverablesAsync(jobOrders, mediaFiles, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"خطأ في ملء البيانات: {ex.Message}");
            return false;
        }
    }

    #region 1. Media Files (الملفات والصور)

    private async Task<List<Media>> CreateMediaFilesAsync(CancellationToken cancellationToken)
    {
        var mediaList = new List<Media>();
        var colors = new[] { Color.Blue, Color.Red, Color.Green, Color.Yellow, Color.Purple, Color.Orange, Color.Pink, Color.Cyan };

        for (int i = 1; i <= 50; i++)
        {
            var fileName = $"image_{i}.png";
            var filePath = Path.Combine(_uploadsPath, fileName);

            // إنشاء صورة PNG بحجم 400x400
            using (var bitmap = new Bitmap(400, 400))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var color = colors[(i - 1) % colors.Length];
                graphics.Clear(color);
                graphics.DrawString($"Image {i}", new Font("Arial", 20), Brushes.White, 150, 180);
                bitmap.Save(filePath, ImageFormat.Png);
            }

            var media = new Media
            {
                FileName = fileName,
                ContentType = "image/png",
                FileExtension = ".png",
                Size = new FileInfo(filePath).Length
            };

            mediaList.Add(media);
        }

        await _db.Medias.AddRangeAsync(mediaList, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return mediaList;
    }

    #endregion

    #region 2. Users & Roles (المستخدمين والأدوار)

    private async Task<List<User>> CreateUsersAndRolesAsync(List<Media> mediaFiles, CancellationToken cancellationToken)
    {
        var users = new List<User>();
        var roles = new[] { "Admin", "Client", "Freelancer", "ServiceProvider" };

        // إنشاء الأدوار
        var roleManager = _db.GetService<RoleManager<Role>>();
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new Role { Name = roleName });
            }
        }

        // إنشاء 20 مستخدم
        for (int i = 1; i <= 20; i++)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = $"user{i}@khdamatk.com",
                UserName = $"user{i}",
                FullName = $"المستخدم {i}",
                EmailConfirmed = true,
                DateOfBirth = new DateTime(1990 + (i % 20), (i % 12) + 1, (i % 28) + 1),
                Role = roles[i % roles.Length],
                Status = i % 3 == 0 ? "Blocked" : "Active",
                IsTrustedByAdmin = i % 5 == 0,
                ProfilePictureId = i % mediaFiles.Count,
                CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(30, 365))
            };

            // تعيين كلمة السر
            var result = await _userManager.CreateAsync(user, "Giggo343@");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, user.Role);
                users.Add(user);
            }
        }

        return users;
    }

    #endregion

    #region 3. Categories & Skills (التصنيفات والمهارات)

    private async Task<List<Category>> CreateCategoriesAsync(CancellationToken cancellationToken)
    {
        var categories = new List<Category>
        {
            new() { Name = "البرمجة", Description = "خدمات البرمجة وتطوير التطبيقات" },
            new() { Name = "التصميم", Description = "خدمات التصميم الجرافيكي والويب" },
            new() { Name = "الكتابة", Description = "خدمات الكتابة والمحتوى" },
            new() { Name = "التسويق", Description = "خدمات التسويق الرقمي والإعلانات" },
            new() { Name = "الاستشارات", Description = "خدمات استشارية متخصصة" },
            new() { Name = "الترجمة", Description = "خد��ات الترجمة والتعريب" },
            new() { Name = "الفيديو", Description = "خدمات تحرير وإنتاج الفيديو" },
            new() { Name = "الموسيقى", Description = "خدمات الموسيقى والصوتيات" }
        };

        await _db.Categories.AddRangeAsync(categories, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return categories;
    }

    private async Task<List<Skill>> CreateSkillsAsync(CancellationToken cancellationToken)
    {
        var skills = new List<Skill>
        {
            // البرمجة
            new() { Name = "C#" }, new() { Name = "ASP.NET" }, new() { Name = "JavaScript" },
            new() { Name = "React" }, new() { Name = "Vue.js" }, new() { Name = "Python" },
            new() { Name = "Java" }, new() { Name = "SQL" }, new() { Name = "MongoDB" },
            
            // التصميم
            new() { Name = "Adobe XD" }, new() { Name = "Figma" }, new() { Name = "Photoshop" },
            new() { Name = "Illustrator" }, new() { Name = "UI Design" }, new() { Name = "UX Design" },
            
            // التسويق
            new() { Name = "SEO" }, new() { Name = "SEM" }, new() { Name = "Social Media" },
            new() { Name = "Email Marketing" }, new() { Name = "Analytics" }, new() { Name = "Content Strategy" },
            
            // الكتابة
            new() { Name = "Copywriting" }, new() { Name = "Blog Writing" }, new() { Name = "Technical Writing" },
            new() { Name = "Creative Writing" }, new() { Name = "SEO Writing" }, new() { Name = "News Writing" },
            
            // الترجمة
            new() { Name = "الترجمة من الإنجليزية" }, new() { Name = "الترجمة من الفرنسية" },
            new() { Name = "تعريب المواقع" }, new() { Name = "ترجمة فنية" }
        };

        await _db.Skills.AddRangeAsync(skills, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return skills;
    }

    #endregion

    #region 4. Service Providers (مقدمو الخدمات)

    private async Task<List<ServiceProviderProfile>> CreateServiceProvidersAsync(
        List<User> users, List<Media> mediaFiles, List<Skill> skills, CancellationToken cancellationToken)
    {
        var providers = new List<ServiceProviderProfile>();
        var freelancers = users.Where(u => u.Role == "Freelancer" || u.Role == "ServiceProvider").Take(10).ToList();

        for (int i = 0; i < freelancers.Count; i++)
        {
            var provider = new ServiceProviderProfile
            {
                UserId = freelancers[i].Id,
                Bio = $"محترف متخصص في المجال رقم {i + 1}",
                HourlyRate = 100 + (i * 50),
                TotalEarnings = _random.Next(5000, 50000),
                TotalHours = _random.Next(100, 1000),
                CompletedProjects = _random.Next(10, 100),
                AverageRating = Math.Round(3.5 + (_random.NextDouble() * 1.5), 1),
                ResponseRate = 85 + _random.Next(15),
                IsVerified = i % 2 == 0,
                VerificationDate = DateTime.UtcNow.AddDays(-30)
            };

            providers.Add(provider);
        }

        await _db.ServiceProviderProfiles.AddRangeAsync(providers, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        // إضافة المهارات لمقدمي الخدمة
        var providerSkills = new List<ProviderSkill>();
        for (int i = 0; i < providers.Count; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                var skill = skills[(i * 5 + j) % skills.Count];
                providerSkills.Add(new ProviderSkill
                {
                    ServiceProviderProfileId = providers[i].UserId,
                    SkillId = skill.Id,
                    MyLevel = (SkillExperienceLevel)(_random.Next(1, 6))
                });
            }
        }

        await _db.ProviderSkills.AddRangeAsync(providerSkills, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return providers;
    }

    #endregion

    #region 5. Services (الخدمات)

    private async Task<List<Service>> CreateServicesAsync(
        List<ServiceProviderProfile> providers, List<Category> categories, List<Media> mediaFiles, CancellationToken cancellationToken)
    {
        var services = new List<Service>();

        for (int i = 0; i < 30; i++)
        {
            var service = new Service
            {
                Title = $"خدمة {i + 1} - {categories[i % categories.Count].Name}",
                ShortDescription = $"وصف قصير للخدمة رقم {i + 1}",
                DetailedDescription = $"وصف مفصل للخدمة رقم {i + 1} مع شرح كامل لما تقدمه",
                Price = 500 + (i * 100),
                DeliveryTimeInDays = 3 + (i % 14),
                AverageRating = Math.Round(3.0 + (_random.NextDouble() * 2.0), 1),
                TotalReviews = _random.Next(5, 50),
                RevisionCount = _random.Next(0, 5),
                CategoryId = categories[i % categories.Count].Id,
                ServiceProviderProfileId = providers[i % providers.Count].UserId,
                MainMediaId = mediaFiles[i % mediaFiles.Count].Id,
                Concepts = new List<string> { "احترافي", "سريع", "جودة عالية" }
            };

            services.Add(service);
        }

        await _db.Services.AddRangeAsync(services, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        // ربط الخدمات بالملفات (ServiceMedia)
        var serviceMediaLinks = new List<ServiceMedia>();
        for (int i = 0; i < services.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                serviceMediaLinks.Add(new ServiceMedia
                {
                    ServiceId = services[i].Id,
                    MediaId = mediaFiles[(i * 3 + j) % mediaFiles.Count].Id
                });
            }
        }

        await _db.ServiceMedia.AddRangeAsync(serviceMediaLinks, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return services;
    }

    #endregion

    #region 6. Certificates & Portfolio (الشهادات والأعمال السابقة)

    private async Task<bool> CreateCertificatesAndPortfolioAsync(
        List<ServiceProviderProfile> providers, List<Media> mediaFiles, CancellationToken cancellationToken)
    {
        var certificates = new List<Certificate>();
        var portfolioItems = new List<PortfolioItem>();

        for (int i = 0; i < providers.Count; i++)
        {
            // الشهادات
            for (int j = 0; j < 3; j++)
            {
                certificates.Add(new Certificate
                {
                    ServiceProviderProfileId = providers[i].UserId,
                    Title = $"شهادة {j + 1}",
                    Issuer = $"جهة معتمدة {j + 1}",
                    Type = "Professional",
                    YearAcquired = 2020 + j,
                    MediaId = mediaFiles[(i * 3 + j) % mediaFiles.Count].Id
                });
            }

            // الأعمال السابقة
            for (int j = 0; j < 5; j++)
            {
                portfolioItems.Add(new PortfolioItem
                {
                    ServiceProviderProfileId = providers[i].UserId,
                    Title = $"مشروع {j + 1}",
                    Description = $"وصف المشروع {j + 1} مع التفاصيل الكاملة",
                    ProjectUrl = $"https://example.com/project{j + 1}",
                    CompletionDate = DateTime.UtcNow.AddMonths(-_random.Next(1, 24)),
                    SchoolName = "جامعة ما",
                    Degree = "بكالوريوس",
                    FieldOfStudy = "هندسة",
                    Company = "شركة ما",
                    StartDate = DateTime.UtcNow.AddYears(-3),
                    EndDate = DateTime.UtcNow.AddYears(-1)
                });
            }
        }

        await _db.Certificates.AddRangeAsync(certificates, cancellationToken);
        await _db.PortfolioItems.AddRangeAsync(portfolioItems, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        // ربط الملفات بالأعم��ل (PortfolioMedia)
        var portfolioMediaLinks = new List<PortfolioMedia>();
        for (int i = 0; i < portfolioItems.Count; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                portfolioMediaLinks.Add(new PortfolioMedia
                {
                    PortfolioItemId = portfolioItems[i].Id,
                    MediaId = mediaFiles[(i * 2 + j) % mediaFiles.Count].Id
                });
            }
        }

        await _db.PortfolioMedia.AddRangeAsync(portfolioMediaLinks, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }

    #endregion

    #region 7. Job Posts & Offers (إعلانات الوظائف والعروض)

    private async Task<List<JobPost>> CreateJobPostsAsync(
        List<User> users, List<Category> categories, List<Skill> skills, List<Media> mediaFiles, CancellationToken cancellationToken)
    {
        var jobPosts = new List<JobPost>();
        var clients = users.Where(u => u.Role == "Client").Take(5).ToList();

        for (int i = 0; i < 15; i++)
        {
            var jobPost = new JobPost
            {
                CustomerId = clients[i % clients.Count].Id,
                CategoryId = categories[i % categories.Count].Id,
                Title = $"وظيفة {i + 1}",
                Description = $"وصف الوظيفة {i + 1} مع متطلبات التنفيذ",
                BudgetMin = 1000 + (i * 500),
                BudgetMax = 3000 + (i * 500),
                Status = JobPostStatus.Open,
                ExperienceLevel = (ExperienceLevel)(1 + (i % 3)),
                ProjectLength = "3-6 أشهر",
                TimeCommitment = (TimeCommit)(i % 4),
                Deadline = DateTime.UtcNow.AddDays(30 + (i * 2)),
                CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(1, 30))
            };

            jobPosts.Add(jobPost);
        }

        await _db.JobPosts.AddRangeAsync(jobPosts, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        // إضافة متطلبات المهارات
        var jobSkillRequirements = new List<JobSkillRequirement>();
        for (int i = 0; i < jobPosts.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var skill = skills[(i * 3 + j) % skills.Count];
                jobSkillRequirements.Add(new JobSkillRequirement
                {
                    JobPostId = jobPosts[i].Id,
                    SkillId = skill.Id,
                    RequiredLevel = (SkillExperienceLevel)(_random.Next(2, 5))
                });
            }
        }

        await _db.JobSkillRequirements.AddRangeAsync(jobSkillRequirements, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        // إضافة المراحل (MileStones)
        var milestones = new List<MileStone>();
        for (int i = 0; i < jobPosts.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                milestones.Add(new MileStone
                {
                    Title = $"مرحلة {j + 1} للوظيفة {i + 1}",
                    Description = $"وصف المرحلة {j + 1}",
                    StepNumber = j + 1,
                    IsCompleted = j < _random.Next(0, 3),
                    Price = 500 + (j * 200)
                });
            }
        }

        await _db.MileStones.AddRangeAsync(milestones, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return jobPosts;
    }

    private async Task<List<JobOffer>> CreateJobOffersAsync(
        List<JobPost> jobPosts, List<ServiceProviderProfile> providers, List<Media> mediaFiles, CancellationToken cancellationToken)
    {
        var jobOffers = new List<JobOffer>();

        for (int i = 0; i < jobPosts.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var offer = new JobOffer
                {
                    JobPostId = jobPosts[i].Id,
                    ProviderProfileId = providers[(i * 3 + j) % providers.Count].UserId,
                    Description = $"عرض {j + 1} للوظيفة {i + 1}",
                    DeliveryTimeInDays = 5 + (j * 3),
                    SimilarWorkExamplesURL = $"https://portfolio.example.com/{i}-{j}",
                    Status = j == 0 ? JobOfferStatus.Accepted : (j == 1 ? JobOfferStatus.Rejected : JobOfferStatus.Pending),
                    ExperienceLevel = (ExperienceLevel)(1 + (j % 3)),
                    Amount = jobPosts[i].BudgetMin + (j * 500),
                    IsAccepted = j == 0,
                    TimeCommitment = (TimeCommit)(j % 4)
                };

                jobOffers.Add(offer);
            }
        }

        await _db.JobOffers.AddRangeAsync(jobOffers, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return jobOffers;
    }

    #endregion

    #region 8. Service & Job Orders (طلبات الخدمات والوظائف)

    private async Task<List<ServiceOrder>> CreateServiceOrdersAsync(
        List<User> users, List<Service> services, CancellationToken cancellationToken)
    {
        var serviceOrders = new List<ServiceOrder>();
        var clients = users.Where(u => u.Role == "Client").ToList();

        for (int i = 0; i < 20; i++)
        {
            var order = new ServiceOrder
            {
                ServiceID = services[i % services.Count].Id,
                CustomerId = clients[i % clients.Count].Id,
                ServiceProviderId = services[i % services.Count].ServiceProviderProfileId,
                Status = (OrderStatus)(i % 11),
                Amount = services[i % services.Count].Price,
                AdditionalDetails = $"تفاصيل إضافية للطلب {i + 1}",
                CompletionDate = i % 2 == 0 ? DateTime.UtcNow.AddDays(_random.Next(1, 14)) : null as DateTime?,
                CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(1, 30))
            };

            serviceOrders.Add(order);
        }

        await _db.ServiceOrders.AddRangeAsync(serviceOrders, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return serviceOrders;
    }

    private async Task<List<JobOrder>> CreateJobOrdersAsync(
        List<User> users, List<JobPost> jobPosts, List<ServiceProviderProfile> providers,
        List<JobOffer> jobOffers, CancellationToken cancellationToken)
    {
        var jobOrders = new List<JobOrder>();
        var acceptedOffers = jobOffers.Where(o => o.IsAccepted).Take(10).ToList();

        for (int i = 0; i < acceptedOffers.Count; i++)
        {
            var jobPost = jobPosts.FirstOrDefault(jp => jp.Id == acceptedOffers[i].JobPostId);
            if (jobPost == null) continue;

            var order = new JobOrder
            {
                JobPostId = acceptedOffers[i].JobPostId,
                CustomerId = jobPost.CustomerId,
                ServiceProviderId = acceptedOffers[i].ProviderProfileId,
                Status = (OrderStatus)(i % 11),
                Amount = acceptedOffers[i].Amount,
                StartDate = DateTime.UtcNow.AddDays(-_random.Next(1, 20)),
                DeadlineDate = DateTime.UtcNow.AddDays(_random.Next(5, 30)),
                CompletionDate = i % 2 == 0 ? DateTime.UtcNow : null as DateTime?,
                CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(1, 30))
            };

            jobOrders.Add(order);
        }

        await _db.JobOrders.AddRangeAsync(jobOrders, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return jobOrders;
    }

    #endregion

    #region 9. Payment Transactions (المعاملات المالية)

    private async Task<bool> CreatePaymentTransactionsAsync(
        List<ServiceOrder> serviceOrders, List<JobOrder> jobOrders, CancellationToken cancellationToken)
    {
        var paymentTransactions = new List<PaymentTransaction>();

        // معاملات الخدمات
        for (int i = 0; i < serviceOrders.Count; i++)
        {
            var transaction = new PaymentTransaction
            {
                ServiceOrderId = serviceOrders[i].Id,
                Amount = serviceOrders[i].Amount,
                PlatformFee = serviceOrders[i].Amount * 0.1m, // 10% رسم المنصة
                NetPayout = serviceOrders[i].Amount * 0.9m,
                Currency = CurrencyCode.EGP,
                Status = (TransactionStatus)(i % 4),
                TransactionDate = DateTime.UtcNow.AddDays(-_random.Next(1, 20)),
                GatewayUsed = (PaymentGateway)(i % 3),
                GatewayReferenceId = $"TXN-{Guid.NewGuid()}"
            };

            paymentTransactions.Add(transaction);
        }

        // معاملات الوظائف
        for (int i = 0; i < jobOrders.Count; i++)
        {
            var transaction = new PaymentTransaction
            {
                JobOrderId = jobOrders[i].Id,
                Amount = jobOrders[i].Amount,
                PlatformFee = jobOrders[i].Amount * 0.15m, // 15% رسم المنصة
                NetPayout = jobOrders[i].Amount * 0.85m,
                Currency = CurrencyCode.EGP,
                Status = (TransactionStatus)(i % 4),
                TransactionDate = DateTime.UtcNow.AddDays(-_random.Next(1, 20)),
                GatewayUsed = (PaymentGateway)(i % 3),
                GatewayReferenceId = $"JOB-{Guid.NewGuid()}"
            };

            paymentTransactions.Add(transaction);
        }

        await _db.PaymentTransactions.AddRangeAsync(paymentTransactions, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }

    #endregion

    #region 10. Credit Cards (البطاقات الائتمانية)

    private async Task<bool> CreateCreditCardsAsync(List<User> users, CancellationToken cancellationToken)
    {
        var creditCards = new List<CreditCard>();

        for (int i = 0; i < users.Count; i++)
        {
            var card = new CreditCard
            {
                UserId = users[i].Id,
                CardholderName = users[i].FullName,
                CardNumber = GenerateCardNumber(),
                ExpiryMonth = _random.Next(1, 13),
                ExpiryYear = 2025 + _random.Next(0, 5),
                CVV = GenerateCVV(),
                IsDefault = i % 3 == 0,
                IsTokenized = i % 2 == 0,
                TokenizedCardReference = $"TOKEN-{Guid.NewGuid()}",
                CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(1, 180))
            };

            creditCards.Add(card);
        }

        await _db.CreditCards.AddRangeAsync(creditCards, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }

    private string GenerateCardNumber()
    {
        var cardNumber = "4";
        for (int i = 0; i < 15; i++)
        {
            cardNumber += _random.Next(0, 10);
        }
        return cardNumber;
    }

    private string GenerateCVV()
    {
        return $"{_random.Next(100, 1000)}";
    }

    #endregion

    #region 11. Conversations & Messages (المحادثات والرسائل)

    private async Task<bool> CreateConversationsAndMessagesAsync(
        List<ServiceOrder> serviceOrders, List<JobOrder> jobOrders, List<User> users, CancellationToken cancellationToken)
    {
        var conversations = new List<Conversation>();
        var messages = new List<Message>();

        // محادثات الخدمات
        for (int i = 0; i < serviceOrders.Count; i++)
        {
            var conversation = new Conversation
            {
                RelatedEntityId = serviceOrders[i].Id,
                Title = $"محادثة الخدمة {i + 1}",
                ServiceOrderId = serviceOrders[i].Id,
                CustomerId = serviceOrders[i].CustomerId,
                ProviderId = serviceOrders[i].ServiceProviderId,
                Category = ConversationCategory.Standard,
                ContextType = ConversationContextType.ServiceOrder,
                CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(1, 20))
            };

            conversations.Add(conversation);
        }

        // محادثات الوظائف
        for (int i = 0; i < jobOrders.Count; i++)
        {
            var conversation = new Conversation
            {
                RelatedEntityId = jobOrders[i].Id,
                Title = $"محادثة الوظيفة {i + 1}",
                JobOrderId = jobOrders[i].Id,
                CustomerId = jobOrders[i].CustomerId,
                ProviderId = jobOrders[i].ServiceProviderId,
                Category = ConversationCategory.Standard,
                ContextType = ConversationContextType.JobOffer,
                CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(1, 20))
            };

            conversations.Add(conversation);
        }

        await _db.Conversations.AddRangeAsync(conversations, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        // الرسائل
        for (int i = 0; i < conversations.Count; i++)
        {
            for (int j = 0; j < _random.Next(3, 10); j++)
            {
                var message = new Message
                {
                    ConversationId = conversations[i].Id,
                    SenderId = j % 2 == 0 ? conversations[i].CustomerId : conversations[i].ProviderId,
                    Content = $"رسالة {j + 1} في المحادثة {i + 1}",
                    IsRead = j < _random.Next(1, 5),
                    CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(0, 15)),
                    CreatedBy = j % 2 == 0 ? conversations[i].CustomerId : conversations[i].ProviderId
                };

                messages.Add(message);
            }
        }

        await _db.Messages.AddRangeAsync(messages, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }

    #endregion

    #region 12. Reviews & Disputes (التقييمات والنزاعات)

    private async Task<bool> CreateReviewsAndDisputesAsync(
        List<ServiceOrder> serviceOrders, List<JobOrder> jobOrders, List<User> users, CancellationToken cancellationToken)
    {
        var reviews = new List<Review>();
        var disputes = new List<Dispute>();

        // التقييمات للخدمات
        var completedServiceOrders = serviceOrders.Where(o => o.Status == OrderStatus.Active || o.Status == OrderStatus.UnderReview).Take(10).ToList();
        for (int i = 0; i < completedServiceOrders.Count; i++)
        {
            var review = new Review
            {
                Title = $"تقييم ممتاز {i + 1}",
                Content = $"خدمة ممتازة وسريعة مع احترافية عالية {i + 1}",
                Rating = 3 + _random.NextDouble() * 2,
                ReviewerId = completedServiceOrders[i].CustomerId,
                ServiceProviderId = completedServiceOrders[i].ServiceProviderId,
                ServiceOrderId = completedServiceOrders[i].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(1, 10)),
                CreatedBy = completedServiceOrders[i].CustomerId
            };

            reviews.Add(review);
        }

        // التقييمات للوظائف
        var completedJobOrders = jobOrders.Where(o => o.Status == OrderStatus.Active || o.Status == OrderStatus.UnderReview).Take(5).ToList();
        for (int i = 0; i < completedJobOrders.Count; i++)
        {
            var review = new Review
            {
                Title = $"تقييم وظيفة ممتازة {i + 1}",
                Content = $"تنفيذ احترافي للوظيفة مع التزام بالمواعيد {i + 1}",
                Rating = 3.5 + _random.NextDouble() * 1.5,
                ReviewerId = completedJobOrders[i].CustomerId,
                ServiceProviderId = completedJobOrders[i].ServiceProviderId,
                JobOrderId = completedJobOrders[i].Id,
                CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(1, 10)),
                CreatedBy = completedJobOrders[i].CustomerId
            };

            reviews.Add(review);
        }

        await _db.Reviews.AddRangeAsync(reviews, cancellationToken);

        // النزاعات
        var conversationsForDisputes = await _db.Conversations.Take(3).ToListAsync(cancellationToken);
        for (int i = 0; i < conversationsForDisputes.Count; i++)
        {
            // إنشاء محادثتين للنزاع (واحدة للرافع وواحدة للمدعى عليه)
            var raiserConversation = new Conversation
            {
                RelatedEntityId = i,
                Title = $"محادثة نزاع - طرف رافع {i + 1}",
                CustomerId = conversationsForDisputes[i].CustomerId,
                ProviderId = conversationsForDisputes[i].ProviderId,
                Category = ConversationCategory.DisputeRaiser,
                ContextType = ConversationContextType.Dispute,
                CreatedAt = DateTime.UtcNow
            };

            var targetConversation = new Conversation
            {
                RelatedEntityId = i,
                Title = $"محادثة نزاع - طرف مدعى عليه {i + 1}",
                CustomerId = conversationsForDisputes[i].CustomerId,
                ProviderId = conversationsForDisputes[i].ProviderId,
                Category = ConversationCategory.DisputeTarget,
                ContextType = ConversationContextType.Dispute,
                CreatedAt = DateTime.UtcNow
            };

            await _db.Conversations.AddRangeAsync(new[] { raiserConversation, targetConversation }, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            var dispute = new Dispute
            {
                ServiceOrderId = i < serviceOrders.Count ? serviceOrders[i].Id : null,
                RaiserId = conversationsForDisputes[i].CustomerId,
                TargetId = conversationsForDisputes[i].ProviderId,
                AdminReviewerId = users.FirstOrDefault(u => u.Role == "Admin")?.Id,
                RaiserConversationId = raiserConversation.Id,
                TargetConversationId = targetConversation.Id,
                Status = DisputeStatus.Opened,
                Type = (DisputeType)(i % 3),
                AmountUnderDispute = 1000 + (i * 500),
                ReasonDetails = $"تفاصيل النزاع رقم {i + 1}",
                CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(1, 5)),
                CreatedBy = conversationsForDisputes[i].CustomerId
            };

            disputes.Add(dispute);
        }

        await _db.Reviews.AddRangeAsync(reviews, cancellationToken);
        await _db.Disputes.AddRangeAsync(disputes, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }

    #endregion

    #region 13. User Favorites & Reports (المفضلة والتقارير)

    private async Task<bool> CreateUserFavoritesAndReportsAsync(
        List<User> users, List<Service> services, CancellationToken cancellationToken)
    {
        var userFavorites = new List<UserFavorites>();

        // إضافة المفضلة
        for (int i = 0; i < users.Count; i++)
        {
            for (int j = 0; j < _random.Next(1, 6); j++)
            {
                var favorite = new UserFavorites
                {
                    UserId = users[i].Id,
                    ServiceId = services[(i * j + j) % services.Count].Id,
                    AddedAt = DateTime.UtcNow.AddDays(-_random.Next(1, 30))
                };

                userFavorites.Add(favorite);
            }
        }

        await _db.UserFavorites.AddRangeAsync(userFavorites, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }

    #endregion

    #region 14. Job Deliverables (المسلمات الوظيفية)

    private async Task<bool> CreateDeliverablesAsync(
        List<JobOrder> jobOrders, List<Media> mediaFiles, CancellationToken cancellationToken)
    {
        var deliverables = new List<JobDeliverable>();

        for (int i = 0; i < jobOrders.Count; i++)
        {
            var deliverable = new JobDeliverable
            {
                JobOrderId = jobOrders[i].Id,
                Description = $"المسلم النهائي للوظيفة {i + 1}",
                CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(0, 10)),
                CreatedBy = jobOrders[i].ServiceProviderId
            };

            // إضافة الملفات المرفقة
            for (int j = 0; j < _random.Next(1, 4); j++)
            {
                deliverable.Attachments.Add(mediaFiles[(i * j + j) % mediaFiles.Count]);
            }

            deliverables.Add(deliverable);
        }

        await _db.JobDeliverables.AddRangeAsync(deliverables, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }

    #endregion
}
