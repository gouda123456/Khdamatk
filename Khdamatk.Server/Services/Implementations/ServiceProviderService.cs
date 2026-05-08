using Khdamatk.Server.Contracts.Home;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Khdamatk.Server.Services.Implementations;

public class ServiceProviderService(Database db) : IServiceProviderService
{
    private readonly Database db = db;


    /// Retrieves the main freelancers list with support for search filters (Category, Name, or Price).

    public async Task<resultBase> FreelancersPage(FreelancerRequest? freelancerRequest, CancellationToken cancellationToken)
    {

        // 1. Fetch sidebar categories for display
        var servicesSidebar = await db.Categories
            .Select(c => new ServicesCard(c.Id.ToString(), c.Name))
            .AsNoTracking()
            .ToListAsync();

        // 2. Build the base query with related entities (User, Skills, Services)
        var query = db.ServiceProviderProfiles
            .Include(u => u.User)
            .Include(u => u.Skills).ThenInclude(s => s.Skill)
            .Include(u => u.Services).ThenInclude(s => s.Category)
            .AsNoTracking();

        // 3. Apply search filters based on request type
        if (!string.IsNullOrWhiteSpace(freelancerRequest.Value))
        {
            query = freelancerRequest.Type.ToLower() switch
            {
                "service" => query.Where(u => u.Services.Any(s => s.Category.Name == freelancerRequest.Value)),
                "freelancer-name" => query.Where(u => u.User.UserName.Contains(freelancerRequest.Value) || u.JobTitle.Contains(freelancerRequest.Value)),
                "price" => freelancerRequest.Value switch
                {
                    "below-50" => query.Where(u => u.HourlyRate < 50),
                    "50-100" => query.Where(u => u.HourlyRate >= 50 && u.HourlyRate <= 100),
                    "100-150" => query.Where(u => u.HourlyRate > 100 && u.HourlyRate <= 150),
                    "above-150" => query.Where(u => u.HourlyRate > 150),
                    _ => query
                },
                _ => query
            };
        }

        // 4. Map results to the required UI structure (FreelancerCards)
        var providers = await query
            .Select(u => new FreelancerCards(
                u.UserId,
                u.User.ProfilePictureId,
                u.User.UserName ?? "Unknown",
                u.JobTitle,
                (double)u.HourlyRate,
                u.Skills.Select(s => s.Skill.Name).ToList()
            ))
            .ToListAsync();

        // 5. Mock Data Fallback: Returns dummy data if database is empty to prevent UI breaking during development
        if (providers.Count < 1)
        {
            providers = new List<FreelancerCards>
            {
                new FreelancerCards("1", 101, "Omnia Salah", "UI/UX Designer", 350.0, new List<string> { "UI", "UX", "Figma" }),
                new FreelancerCards("2", 102, "Youssef Ashraf", "Full Stack Developer", 500.0, new List<string> { "C#", "SQL", "React" })
            };

            if (servicesSidebar.Count < 1)
            {
                servicesSidebar = new List<ServicesCard> { new("1", "Developers"), new("2", "Designers") };
            }
        }

        var resultData = new Freelancers(providers, servicesSidebar);
        return Success(StatusCodes.Status200OK, resultData);
    }

    /// Retrieves full profile details for a specific service provider, including skills, portfolio, and certificates.
    /// Retrieves full profile details for a specific service provider, including skills, portfolio, and certificates.
    public async Task<resultBase> FreelancerProfile(string userId, CancellationToken cancellationToken)
    {
        // 1. محاولة جلب البيانات الحقيقية
        var profile = await db.ServiceProviderProfiles
            .Include(u => u.User)
            .Include(u => u.Skills).ThenInclude(s => s.Skill)
            .Include(u => u.Certificates)
            .Include(u => u.PortfolioItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        // 2. لو مفيش داتا (عشان الفرونت يشتغل) بنبعت Mock Data بناءً على الـ ID
        if (profile == null)
        {
            if (userId == "1") // Mock Data لأمنية صلاح
            {
                var mockOmnia = new FreelancerProfileResponse1(
                    "1",
                    "Omnia Salah",
                    "UI/UX Designer",
                    "Cairo, Egypt",
                    DateTime.Now.ToString("yyyy MMM"),
                    4.8,
                    2,
                    "Available",
                    "Specialized in creating user-centric designs and wireframes.",
                    350.0,
                    new List<SkillDto> {
                    new SkillDto(1, "UI"),
                    new SkillDto(2, "UX"),
                    new SkillDto(3, "Figma"),
                    new SkillDto(4, "Adobe XD")
                    },
                    new List<_PortfolioItem> { new _PortfolioItem("E-commerce App", "https://behance.net", new List<string> { "Full UI kit for a shopping app." }) },
                    new List<EducationItem> { new EducationItem("Faculty of Applied Arts", "Bachelor's Degree", "Design", "2020-2024") },
                    new List<CertificationItem> { new CertificationItem("Google UX Design", "Coursera", "2025") },
                    new List<ExperienceItem> { new ExperienceItem("Design Agency", "Junior Designer") },
                    null,
                    null
                );
                return Success(StatusCodes.Status200OK, mockOmnia);
            }
            else // Mock Data ليوسف أشرف (الافتراضي)
            {
                var mockYoussef = new FreelancerProfileResponse1(
                    userId,
                    "Youssef Ashraf",
                    ".NET Backend Developer",
                    "Beni Suef, Egypt",
                    DateTime.Now.ToString("yyyy MMM"),
                    5.0,
                    3,
                    "Available",
                    "Backend developer experienced in ASP.NET Core and SQL Server.",
                    500.0,
                    new List<SkillDto> {
                    new SkillDto(5, "C#"),
                    new SkillDto(6, "ASP.NET Core"),
                    new SkillDto(7, "SQL Server"),
                    new SkillDto(8, "React")
                    },
                    new List<_PortfolioItem> { new _PortfolioItem("Shipping System", "https://github.com", new List<string> { "A real-time tracking web system." }) },
                    new List<EducationItem> { new EducationItem("HTI Beni Suef", "Bachelor's Degree", "Computer Science", "2021-2025") },
                    new List<CertificationItem> { new CertificationItem("AI Ambassadors", "NTI - Batch 6", "2026") },
                    new List<ExperienceItem> { new ExperienceItem("Talabeyah", "Sales Representative") },
                    null,
                    null
                );
                return Success(StatusCodes.Status200OK, mockYoussef);
            }
        }

        // 3. لو فيه داتا حقيقية، بنعمل Mapping عادي
        var education = profile.PortfolioItems
            .Where(p => !string.IsNullOrEmpty(p.SchoolName))
            .Select(p => new EducationItem(p.SchoolName!, p.Degree ?? "", p.Description ?? "", $"{p.StartDate:yyyy/M/d} - {p.EndDate:yyyy/M/d}"))
            .ToList();

        var experiences = profile.PortfolioItems
            .Where(p => !string.IsNullOrEmpty(p.Company))
            .Select(p => new ExperienceItem(p.Company!, p.Description ?? ""))
            .ToList();

        var portfolio = profile.PortfolioItems
            .Where(p => string.IsNullOrEmpty(p.SchoolName) && string.IsNullOrEmpty(p.Company))
            .Select(p => new _PortfolioItem(p.Title, p.ProjectUrl, new List<string> { p.Description ?? "" }))
            .ToList();

        var response = new FreelancerProfileResponse1(
            profile.UserId,
            profile.User.UserName ?? "Unknown",
            profile.JobTitle,
            "Cairo, Egypt",
            profile.DateOfJoin.ToString("yyyy MMM"),
            4.5,
            profile.ExperienceYears,
            "Flexible hours",
            profile.Bio ?? "",
            (double)profile.HourlyRate,
            profile.Skills.Select(s => new SkillDto(s.SkillId, s.Skill.Name)).ToList(), // تحويل السكيلز لـ DTO
            portfolio,
            education,
            profile.Certificates.Select(c => new CertificationItem(c.Title, $"{c.Issuer} - {c.Type}", c.YearAcquired.ToString())).ToList(),
            experiences,
            profile.FacebookUrl,
            profile.GithubUrl
        );

        return Success(StatusCodes.Status200OK, response);
    }

    public async Task<resultBase> UpdateProfileBasicInfo(string userId, UpdateProfileRequest request)
    {
        // 1. اتأكد الأول إن الـ ID موجود
        if (string.IsNullOrEmpty(userId))
            return Failure(StatusCodes.Status401Unauthorized, "Error", "User ID is missing.");

        // 2. دور في الداتابيز
        var profile = await db.ServiceProviderProfiles.FirstOrDefaultAsync(p => p.UserId == userId);

        // 3. لو مفيش كـريه واحد جديد (Logic الـ Upsert)
        if (profile == null)
        {
            profile = new Khdamatk.Server.Data.Entities.Identity.ServiceProviderProfile
            {
                UserId = userId,
                DateOfJoin = DateTime.Now
            };
            await db.ServiceProviderProfiles.AddAsync(profile);
        }

        // 4. حدث البيانات
        profile.JobTitle = request.JobTitle;
        profile.Bio = request.Bio;
        profile.HourlyRate = (double)request.HourlyRate;
        profile.ExperienceYears = request.ExperienceYears;
        profile.ProfileImageUrl = request.ProfileImageUrl;
        profile.CoverImageUrl = request.CoverImageUrl;
        profile.FacebookUrl = request.FacebookUrl;
        profile.LinkedInUrl = request.LinkedInUrl;
        profile.GithubUrl = request.GithubUrl;

        await db.SaveChangesAsync();
        return Success(StatusCodes.Status200OK, "Profile updated successfully");
    }

    /// Adds a new project item to the provider's portfolio.
    public async Task<resultBase> AddPortfolioItem(string userId, AddPortfolioRequest request)
    {
        var newItem = new Khdamatk.Server.Data.Entities.Catalog.PortfolioItem
        {
            ServiceProviderProfileId = userId,
            Title = request.Title,
            Description = request.Description,
            ProjectUrl = request.ImageUrl,
            CompletionDate = DateTime.UtcNow
        };

        await db.PortfolioItems.AddAsync(newItem);
        await db.SaveChangesAsync();
        return Success(StatusCodes.Status200OK, "Added successfully");
    }

    public async Task<resultBase> AddEducation(string userId, AddEducationRequest request)
    {
        var profile = await db.ServiceProviderProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null) return Failure(StatusCodes.Status404NotFound, "Error", "Profile not found");

        // 1. بندور على أي سجلات تعليم قديمة للمستخدم ده ونمسحها الأول
        var oldEducation = db.PortfolioItems.Where(p => p.ServiceProviderProfileId == userId);
        if (oldEducation.Any())
        {
            db.PortfolioItems.RemoveRange(oldEducation);
        }

        // 2. بنضيف السجل الجديد اللي جاي في الـ Request
        var education = new Khdamatk.Server.Data.Entities.Catalog.PortfolioItem
        {
            ServiceProviderProfileId = userId,
            SchoolName = request.SchoolName,
            Degree = request.Degree,
            FieldOfStudy = request.FieldOfStudy,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        await db.PortfolioItems.AddAsync(education);

        // 3. بنحفظ التغييرات (المسح والإضافة بيحصلوا في خطوة واحدة)
        await db.SaveChangesAsync();

        return Success(StatusCodes.Status201Created, "Education updated successfully");
    }

    /// Adds work experience records to the provider's profile.
    public async Task<resultBase> AddExperience(string userId, AddExperienceRequest request)
    {
        var profile = await db.ServiceProviderProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null) return Failure(StatusCodes.Status404NotFound, "Error", "Profile not found");

        var experience = new Khdamatk.Server.Data.Entities.Catalog.PortfolioItem
        {
            ServiceProviderProfileId = userId,
            Title = request.Title,
            Company = request.CompanyName,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        await db.PortfolioItems.AddAsync(experience);
        await db.SaveChangesAsync();
        return Success(StatusCodes.Status201Created, "Experience added successfully");
    }

    /// Deletes a specific item (Portfolio, Education, or Experience) by ID.
    public async Task<resultBase> DeletePortfolioItem(string userId, int itemId)
    {
        var item = await db.PortfolioItems
            .FirstOrDefaultAsync(p => p.Id == itemId && p.ServiceProviderProfileId == userId);

        if (item == null) return Failure(StatusCodes.Status404NotFound, "Error", "Item not found");

        db.PortfolioItems.Remove(item);
        await db.SaveChangesAsync();
        return Success(StatusCodes.Status200OK, "Deleted successfully");
    }

    /// Updates the provider's skill set by clearing existing skills and adding the newly provided ones.
    public async Task<resultBase> UpdateSkills(string userId, UpdateSkillsRequest request)
    {
        var profile = await db.ServiceProviderProfiles
            .Include(p => p.Skills)
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null) return Failure(StatusCodes.Status404NotFound, "Error", "Profile not found");

        // Clear existing skill links
        profile.Skills.Clear();

        // Add new skill links
        foreach (var skillId in request.SkillIds)
        {
            profile.Skills.Add(new ProviderSkill
            {
                SkillId = skillId,
                ServiceProviderProfileId = userId
            });
        }

        await db.SaveChangesAsync();
        return Success(StatusCodes.Status200OK, "Skills updated successfully");
    }

    /// Adds a professional certificate or license record to the profile.
    public async Task<resultBase> AddCertificate(string userId, AddCertificateRequest request)
    {
        var profile = await db.ServiceProviderProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null) return Failure(StatusCodes.Status404NotFound, "Error", "Profile not found");

        var certificate = new Certificate
        {
            ServiceProviderProfileId = userId,
            Title = request.Title,
            Issuer = request.Issuer,
            Type = request.Type,
            YearAcquired = request.YearAcquired
        };

        await db.Certificates.AddAsync(certificate);
        await db.SaveChangesAsync();
        return Success(StatusCodes.Status201Created, "Certificate added successfully");
    }

    /// Updates an existing item (Portfolio/Education/Experience) in the PortfolioItems table.
    public async Task<resultBase> UpdatePortfolioItem(string userId, int itemId, AddPortfolioRequest request)
    {
        var item = await db.PortfolioItems.FirstOrDefaultAsync(p => p.Id == itemId && p.ServiceProviderProfileId == userId);
        if (item == null) return Failure(StatusCodes.Status404NotFound, "Error", "Item not found");

        item.Title = request.Title;
        item.Description = request.Description;
        item.ProjectUrl = request.ImageUrl; // This can also store SchoolName/Company if logic requires

        await db.SaveChangesAsync();
        return Success(StatusCodes.Status200OK, "Item updated successfully");
    }

    /// Deletes an education record using the general portfolio deletion logic.
    public async Task<resultBase> DeleteEducation(string userId, int eduId) => await DeletePortfolioItem(userId, eduId);


    /// Deletes an experience record using the general portfolio deletion logic.

    public async Task<resultBase> DeleteExperience(string userId, int expId) => await DeletePortfolioItem(userId, expId);
}