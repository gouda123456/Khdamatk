using Khdamatk.Server.Contracts.Home;

namespace Khdamatk.Server.Services.Implementations;
public class HomeService(Database db) : IHomeService
{
    private readonly Database db = db;



    //TODO: Send Picture URL instead of PictureId
    //TODO: Implement Caching for this method
    //TODO: Implement Pagination for this method
    //TODO: Implement Filtering for this method
    //TODO: Implement Sorting for this method
    //TODO: Implement Localization for this method
    //TODO: Implement Error Handling for this method
    //TODO: Implement Logging for this method
    //TODO: Implement Unit Tests for this method
    //TODO: Test the Query , optimize it if needed , and make sure it works as expected , add Fake Data (hundred of Rows )
    //TODO: Review the Code and make sure it follows the best practices
    ///////////MainPage////////////////
    public async Task<resultBase> MainPage(CancellationToken cancellationToken)
    {
        var Categories = await db.Categories.Select(c => c.Name).AsNoTracking().Take(10).ToListAsync();
        
        var FreelancerCards = await db.ServiceProviderProfiles.Include(u => u.User)
            .Select(u => new FreelancerCard(u.UserId,
            u.User.ProfilePictureId,
            u.User.UserName?? "UnKnown",
            u.JobTitle,
            u.HourlyRate,
            u.Skills.Select(s => s.Skill.Name)
            .ToList()?? new List<string>() {"there are no skill" }))
            .Take(10)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var ClientReviewCard = await db.Reviews.Include(r => r.Reviewer)
            .Select(r => new ClientReviewCard(
            r.Reviewer.ProfilePictureId,
            r.Reviewer.UserName ?? "Unknown",
            r.Content,
            r.Rating,
            "Client")
            ).Take(5)
            .AsNoTracking()
            .ToListAsync();

        if (Categories == null || FreelancerCards == null || ClientReviewCard == null)
        {
            return Failure(StatusCodes.Status404NotFound, FailureMessages.DataNotFound.Title,FailureMessages.DataNotFound.Message);
        }

        if (Categories.Count == 0 || FreelancerCards.Count == 0 || ClientReviewCard.Count == 0)
        {
            Categories = ["no categories", "no categories2"];
            FreelancerCards = [new FreelancerCard("string Id",123,"user name", "Jobtitle", 12.5, ["skill1","skill2"]), new FreelancerCard("string Id2", 12, "user name2", "Jobtitle2", 15, ["skil3", "skill4"])];
            ClientReviewCard = [new (1,"ClientName","review text",4.2), new(2, "ClientName2", "review text2", 3)];
            return Success(StatusCodes.Status200OK, new MainPage(Categories, FreelancerCards, ClientReviewCard));
            return Failure(StatusCodes.Status204NoContent, FailureMessages.DataNotAvailable.Title,FailureMessages.DataNotAvailable.Message);
        }

        return Success(StatusCodes.Status200OK, new MainPage(Categories, FreelancerCards, ClientReviewCard));
    }
    ////////////////////JobsPage///////////////
    public async Task<resultBase> JobsPage(string? service, CancellationToken cancellationToken)
    {
       return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message); 
    }
    ////////////////////FreelancersPage///////////////
    public async Task<resultBase> FreelancersPage(FreelancerRequest freelancerRequest, CancellationToken cancellationToken)
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


        /////////////////*************///////////////////

    public async Task<resultBase> UpdateProfileBasicInfo(string userId, UpdateProfileRequest request)
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

         /////////////////**************///////////

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

           ////////////////////***********///////////////

    public async Task<resultBase> AddEducation(string userId, AddEducationRequest request)
    {
        // 1. نتأكد إن البروفايل موجود
        var profile = await db.ServiceProviderProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null)
            return Failure(StatusCodes.Status404NotFound, "Error", "Profile not found");

        // 2. إضافة السجل الجديد (بافتراض اسم الـ Entity عندك Education)
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

    /////////////////////*************///////////////

    public async Task<resultBase> AddExperience(string userId, AddExperienceRequest request)
    {
        // 1. التأكد من وجود البروفايل
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
        // 1. بندور على العمل بالـ ID وبنتأكد إنه بتاع اليوزر ده
        var item = await db.PortfolioItems
            .FirstOrDefaultAsync(p => p.Id == itemId && p.ServiceProviderProfileId == userId);

        // 2. لو مش موجود أو مش بتاعه
        if (item == null)
            return Failure(StatusCodes.Status404NotFound, "Error", "Item not found or you don't have permission to delete it");

        // 3. المسح من الداتابيز
        db.PortfolioItems.Remove(item);
        await db.SaveChangesAsync();

        return Success(StatusCodes.Status200OK, "Project deleted successfully from your portfolio");
    }
    public async Task<resultBase> UpdateSkills(string userId, UpdateSkillsRequest request)
    {
        // 1. بنجيب البروفايل بالمهارات الحالية بتاعته
        var profile = await db.ServiceProviderProfiles
            .Include(p => p.Skills)
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null)
            return Failure(StatusCodes.Status404NotFound, "Error", "Profile not found");

        // 2. بنمسح المهارات القديمة المرتبطة بالبروفايل ده
        profile.Skills.Clear();

        // 3. بنضيف المهارات الجديدة من الـ IDs اللي جاية في الـ Request
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


