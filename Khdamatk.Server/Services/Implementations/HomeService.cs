
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
    
    public async Task<resultBase> JobsPage(string? service, CancellationToken cancellationToken)
    {
       return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message); 
    }
    
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
}


