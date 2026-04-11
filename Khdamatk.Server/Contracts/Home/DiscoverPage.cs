namespace Khdamatk.Server.Contracts.Home
{
    public record DiscoverPage
   (
        List <DiscoverServiceProvider> ServiceProviders,
        List <DiscoverJobPost> JobPosts
    );

    public record DiscoverServiceProvider
        (
        string Id,
        string Name,
        string Pic
        );
    public record DiscoverJobPost
        (
        int Id,
        string Title,
        string PostedTime,
        string Budget,
        string Description,
        List<string> JobSkillRequirement
        );
}
