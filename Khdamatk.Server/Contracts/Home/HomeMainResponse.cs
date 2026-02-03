namespace Khdamatk.Server.Contracts.Home
{
    public record HomeMainResponse(List<string> Categories,List<FreelancerHomePageCard> Freelancers);
    public record FreelancerHomePageCard(string ProfileImagePath , string Name, string job );
}
