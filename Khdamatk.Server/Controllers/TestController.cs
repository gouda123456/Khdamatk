using Asp.Versioning;
using System.Globalization;
using Khdamatk.Server.Contracts.Fawaterak;
using Khdamatk.Server.Helper.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers;

[Route("api/[controller]")]
[ApiController]

public class TestController : ControllerBase
{
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
    public async Task<IActionResult> TestFileDownload([FromServices] Database db)
    {
        var media = db.Medias.FirstOrDefault();
        var path = media.FullPath;
        return Ok(path);
    }

    [HttpPost("test-file-upload")]
    public async Task<IActionResult> TestFileUploadPost([FromServices] Database db, IFormFile file)
    {
        var media = await FileManagement.UploadFileAsync(file);
        db.Medias.Add(media);
        await db.SaveChangesAsync();
        return Ok(media);
    }

    [HttpPost("SeedData")]
    public async Task<IActionResult> SeedData(
        [FromServices] Database db,
        [FromServices] UserManager<User> userManager,
        [FromServices] IWebHostEnvironment env)
    {
        try
        {
            var mediaList = await GetOrCreateSeedMediaAsync(db, env);
            var categories = await GetOrCreateSeedCategoriesAsync(db);
            var skills = await GetOrCreateSeedSkillsAsync(db);
            var users = await GetOrCreateSeedUsersAsync(userManager, mediaList);
            var providers = await GetOrCreateSeedProviderProfilesAsync(db, users, mediaList, skills);
            var services = await GetOrCreateSeedServicesAsync(db, providers, categories, mediaList);
            var jobPosts = await GetOrCreateSeedJobPostsAsync(db, users, categories);
            await GetOrCreateSeedJobSkillRequirementsAsync(db, jobPosts, skills);
            var serviceOrders = await GetOrCreateSeedServiceOrdersAsync(db, services, users, providers);
            await GetOrCreateSeedReviewsAsync(db, serviceOrders, providers);
            await EnsureSeedExtensionEntitiesAsync(db, users, services, serviceOrders, jobPosts, mediaList);

            return Ok(new
            {
                Message = "Seed completed (idempotent). Safe to call multiple times.",
                Notes =
                    $"{SeedMarker} marks seeded rows. JobOffer/JobOrder remain skipped (circular FK). Investigation entity is not mapped to EF. DeliveredJobFile is included when milestones exist.",
                Stats = new
                {
                    Users = users.Count,
                    Categories = categories.Count,
                    Skills = skills.Count,
                    Providers = providers.Count,
                    Services = services.Count,
                    JobPosts = jobPosts.Count,
                    ServiceOrders = serviceOrders.Count,
                    Media = mediaList.Count,
                    JobSkillRequirements = await db.JobSkillRequirements.CountAsync(j => jobPosts.Select(p => p.Id).Contains(j.JobPostId)),
                    Reviews = await db.Reviews.CountAsync(r => r.Title.StartsWith(SeedMarker))
                }
            });
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

    #region Helper Methods for Seeding

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

    private async Task<List<Category>> GetOrCreateSeedCategoriesAsync(Database db)
    {
        var definitions = new (string Name, string Description)[]
        {
            ("برمجة وتطوير", "خدمات البرمجة وتطوير المواقع والتطبيقات"),
            ("تصميم جرافيك", "تصميم الشعارات والهويات البصرية"),
            ("كتابة وترجمة", "كتابة المحتوى والترجمة"),
            ("تسويق رقمي", "التسويق عبر وسائل التواصل الاجتماعي"),
            ("فيديو وصوت", "مونتاج الفيديو والتعليق الصوتي"),
            ("أعمال", "الاستشارات الإدارية والمالية")
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

    #endregion
}