using System.Collections.Generic;

namespace Khdamatk.Server.Contracts.Home;

    public record Freelancers(
        List<FreelancerCards> Providers,
        List<ServicesCard> ServicesCard
    );

    public record FreelancerCards(
    string Id,
    string? ProfilePictureUrl,
    string UserName,
    string JobTitle,
    double HourlyRate,
    List<string> Skills
);

public record ServicesCard(
    string ServiceId,
    string NameService

);


public record FreelancerRequest(
    string? Type,
    string? Value

);

/*Discover
     * {freelancer(Id ,name, pic )
     * JobDiscover(Title, budget, ,list<string> Skills)
     * }
     */