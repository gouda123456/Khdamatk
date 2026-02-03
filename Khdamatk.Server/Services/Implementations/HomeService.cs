
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
        var Categories = await db.Categories.Select(c => c.Name).AsNoTracking().Take(10).ToListAsync(cancellationToken);
        
        var FreelancerCards = await db.ServiceProviderProfiles.Include(u => u.User)
            .Select(u => new FreelancerCard(u.UserId,
            u.User.ProfilePictureId,
            u.User.UserName?? "UnKnown",
            u.JobTitle,
            u.HourlyRate,
            u.Skills.Select(s => s.Name)
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
}
