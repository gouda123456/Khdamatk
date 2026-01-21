namespace Khdamatk.Server.Contracts.Home;

public record MainPage(
    List<string> ServicesCategories,
    List<FreelancerCard> Providers,
    List<ClientReviewCard> ClientReviews 
);

public record FreelancerCard(
    string Id,
    int? ProfilePictureUrl,
    string UserName,
    string JobTitle,
    double HourlyRate,
    List<string> Skills
);

public record ClientReviewCard(
    int? ClientProfilePictureUrl,
    string ClientName,
    string ReviewText,
    double Rating,
    string Jobtitle = "Normal User" 
);
