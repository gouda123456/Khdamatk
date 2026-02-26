using Khdamatk.Server.Contracts.Home;

namespace Khdamatk.Server.Services.Interfaces;

public interface IServiceProviderService : IService
{
    Task<resultBase> FreelancersPage(FreelancerRequest? freelancerRequest, CancellationToken cancellationToken);
    Task<resultBase> FreelancerProfile(string userId, CancellationToken cancellationToken);
    Task<resultBase> UpdateProfileBasicInfo(string? userId, UpdateProfileRequest request);
    Task<resultBase> AddPortfolioItem(string userId, AddPortfolioRequest request);
    Task<resultBase> AddEducation(string userId, AddEducationRequest request);
    Task<resultBase> AddExperience(string userId, AddExperienceRequest request);
    Task<resultBase> DeletePortfolioItem(string userId, int itemId);
    Task<resultBase> UpdateSkills(string userId, UpdateSkillsRequest request);
}
