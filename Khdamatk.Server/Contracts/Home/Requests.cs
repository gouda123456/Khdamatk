namespace Khdamatk.Server.Contracts.Home;


public record AddPortfolioRequest(
    string Title,
    string Description,
    string ImageUrl
);



public record AddEducationRequest(
    string SchoolName,      
    string Degree,          
    string FieldOfStudy,    
    string Description,     
    DateTime StartDate,    
    DateTime? EndDate      
);


public record AddExperienceRequest(
    string Title,          
    string CompanyName,     
    string Description,     
    DateTime StartDate,    
    DateTime? EndDate       
);
public record UpdateProfileRequest(
    string JobTitle,
    string? Bio,
    double HourlyRate,
    int ExperienceYears
);
public record UpdateSkillsRequest(
    List<int> SkillIds 
);