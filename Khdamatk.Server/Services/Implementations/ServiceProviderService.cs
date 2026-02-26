using Khdamatk.Server.Contracts.Home;

namespace Khdamatk.Server.Services.Implementations;

public class ServiceProviderService(Database db) : IServiceProviderService
{
    private readonly Database db = db;

    ////////////////////FreelancersPage///////////////
    public async Task<resultBase> FreelancersPage(FreelancerRequest? freelancerRequest, CancellationToken cancellationToken)
    {

        var servicesSidebar = await db.Categories
            .Select(c => new ServicesCard(c.Id.ToString(), c.Name))
            .AsNoTracking()
            .ToListAsync(cancellationToken);


        var query = db.ServiceProviderProfiles
            .Include(u => u.User)
            .Include(u => u.Skills).ThenInclude(s => s.Skill)
            .Include(u => u.Services).ThenInclude(s => s.Category)
            .AsNoTracking();

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

        var providers = await query
            .Select(u => new FreelancerCards(
                u.UserId,
                u.User.ProfilePictureId,
                u.User.UserName ?? "Unknown",
                u.JobTitle,
                (double)u.HourlyRate,
                u.Skills.Select(s => s.Skill.Name).ToList()
            ))
            .ToListAsync(cancellationToken);

        if (providers.Count < 1)
        {
            providers = new List<FreelancerCards>
        {
            new FreelancerCards("1", 101, "Omnia Salah", "UI/UX Designer", 350.0, new List<string> { "UI", "UX", "Figma" }),
            new FreelancerCards("2", 102, "Youssef Ashraf", "Full Stack Developer", 500.0, new List<string> { "C#", "SQL", "React" }),
            new FreelancerCards("3", 103, "Gouda George", "Digital Marketer", 250.0, new List<string> { "SEO", "Ads", "Content" }),
            new FreelancerCards("4", 104, "Mohamed Hassan", "Graphic Designer", 300.0, new List<string> { "Photoshop", "AI", "Branding" }),
            new FreelancerCards("5", 105, "Youssef Nabil", "Translator", 200.0, new List<string> { "English", "Arabic", "French" }),
            new FreelancerCards("6", 106, "Omnia Salah", "UI/UX Designer", 350.0, new List<string> { "UI", "UX", "Figma" })
        };

            if (servicesSidebar.Count < 1)
            {
                servicesSidebar = new List<ServicesCard>
            {
                new ("1", "Developers"),
                new ("2", "Designers"),
                new ("3", "Translators"),
                new ("4", "Writing"),
                new ("5", "Digital Marketing")
            };
            }
        }

        var resultData = new Freelancers(providers, servicesSidebar);

        return Success(StatusCodes.Status200OK, resultData);
    }
    //////////FreelancerProfile////////
    public async Task<resultBase> FreelancerProfile(string userId, CancellationToken cancellationToken)
    {

        var profile = await db.ServiceProviderProfiles
            .Include(u => u.User)
            .Include(u => u.Skills).ThenInclude(s => s.Skill)
            .Include(u => u.Certificates)
            .Include(u => u.PortfolioItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);


        if (profile == null)
        {
            var fakeProfile = new FreelancerProfileResponse(
                UserId: Guid.NewGuid().ToString(),
                FullName: "Omnia Salah",
                JobTitle: "Software engineer",
                Location: "Cairo, Egypt",
                MemberSince: "2023 Nov",
                Rating: 4.5,
                YearsOfExperience: 2,
                WorkingHours: "Working 3 hours a week as a freelancer",
                Bio: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                HourlyRate: 50.0,
                Skills: new List<string> { "Skill1", "Skill2" },
                Portfolio: new List<_PortfolioItem>
                {
                new ("Name Work", null, new List<string>{"v1", "v2", "v3"}),
                new ("Name Work", null, new List<string>{"v1", "v2", "v3"})
                },
                Education: new List<EducationItem>
                {
                new ("Educational Name", "Specialty", "Lorem ipsum description...", "2022/2/1 - 2026/5/1")
                },
                Certifications: new List<CertificationItem>
                {
                new ("Certification Name", "Lorem ipsum description...", "2022/2/1 - 2026/5/1")
                },
                Experiences: new List<ExperienceItem>
                {
                new ("Name: Experience", "Lorem ipsum description...")
                },
                ProfilePictureUrl: null,
                CoverPictureUrl: null
            );

            return Success(StatusCodes.Status200OK, fakeProfile);
        }

        var response = new FreelancerProfileResponse(
            profile.UserId,
            profile.User.UserName ?? "Unknown",
            profile.JobTitle,
            "Cairo, Egypt",
            profile.DateOfJoin.ToString("yyyy MMM"),
            4.5,
            2,
            "Flexible hours",
            profile.Bio ?? "",
            (double)profile.HourlyRate,
            profile.Skills.Select(s => s.Skill.Name).ToList(),
            profile.PortfolioItems.Select(p => new _PortfolioItem(p.Title, null, new List<string> { p.Description ?? string.Empty })).ToList(),

            new List<EducationItem>(),

            profile.Certificates.Select(c => new CertificationItem(c.Title, $"{c.Issuer} - {c.Type}", c.YearAcquired.ToString())).ToList(),

            new List<ExperienceItem>(),
            null,
            null
        );

        return Success(StatusCodes.Status200OK, response);
    }


    /////////////////UpdateProfileBasicInfo///////////////////

    public async Task<resultBase> UpdateProfileBasicInfo(string? userId, UpdateProfileRequest request)
    {
        var profile = await db.ServiceProviderProfiles.FirstOrDefaultAsync(p => p.UserId == userId);


        if (profile == null)
            return Failure(StatusCodes.Status404NotFound, "Error", "profile is not found");

        profile.JobTitle = request.JobTitle;
        profile.Bio = request.Bio;
        profile.HourlyRate = request.HourlyRate;
        profile.ExperienceYears = request.ExperienceYears;

        await db.SaveChangesAsync();
        return Success(StatusCodes.Status200OK, "The data has been updated effectively");
    }

    /////////////////AddPortfolioItem///////////

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

    ////////////////////AddEducation///////////////

    public async Task<resultBase> AddEducation(string userId, AddEducationRequest request)
    {
       
        var profile = await db.ServiceProviderProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null)
            return Failure(StatusCodes.Status404NotFound, "Error", "Profile not found");

        
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
        await db.SaveChangesAsync();

        return Success(StatusCodes.Status201Created, "Education added successfully");
    }

    /////////////////////AddExperience///////////////

    public async Task<resultBase> AddExperience(string userId, AddExperienceRequest request)
    {
       
        var profile = await db.ServiceProviderProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null)
            return Failure(StatusCodes.Status404NotFound, "Error", "Profile not found");


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
    public async Task<resultBase> DeletePortfolioItem(string userId, int itemId)
    {
      
        var item = await db.PortfolioItems
            .FirstOrDefaultAsync(p => p.Id == itemId && p.ServiceProviderProfileId == userId);

        
        if (item == null)
            return Failure(StatusCodes.Status404NotFound, "Error", "Item not found or you don't have permission to delete it");

       
        db.PortfolioItems.Remove(item);
        await db.SaveChangesAsync();

        return Success(StatusCodes.Status200OK, "Project deleted successfully from your portfolio");
    }
    public async Task<resultBase> UpdateSkills(string userId, UpdateSkillsRequest request)
    {
        
        var profile = await db.ServiceProviderProfiles
            .Include(p => p.Skills)
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null)
            return Failure(StatusCodes.Status404NotFound, "Error", "Profile not found");

        
        profile.Skills.Clear();

  
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
}
