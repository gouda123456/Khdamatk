using System.Collections.Generic;

namespace Khdamatk.Server.Contracts.Home;

    public record Freelancers(

        List<FreelancerCards> Providers,
        List<ServicesCard> cervicescard

    );

    public record FreelancerCards(
    string Id,
    int? ProfilePictureUrl,
    string UserName,
    string JobTitle,
    double HourlyRate,
    List<string> Skills
);
public record ServicesCard(
    string ServiceId,
    string NameService

);

