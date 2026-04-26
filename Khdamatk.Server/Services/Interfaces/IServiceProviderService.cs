using Khdamatk.Server.Contracts.Home;

namespace Khdamatk.Server.Services.Interfaces;

 
/// Interface for Managing Service Provider (Freelancer) profiles, 
/// including searching, profile details, and CRUD operations for portfolio/CV.
public interface IServiceProviderService : IService
{
    // --- Data Retrieval ---
    /// Retrieves a paginated/filtered list of freelancers for the discovery page.
    Task<resultBase> FreelancersPage(FreelancerRequest? freelancerRequest, CancellationToken cancellationToken);


    /// Gets the full profile details for a specific freelancer by their User ID.
    Task<resultBase> FreelancerProfile(string userId, CancellationToken cancellationToken);

    // --- Basic Info & Skills ---
    /// Updates core profile data such as Job Title, Bio, Hourly Rate, and Social Links.
    Task<resultBase> UpdateProfileBasicInfo(string userId, UpdateProfileRequest request);

     
    /// Replaces the freelancer's current skills with a new set of selected skill IDs.
    Task<resultBase> UpdateSkills(string userId, UpdateSkillsRequest request);

    // --- Portfolio Management ---
    /// Adds a new project work to the freelancer's portfolio.
    Task<resultBase> AddPortfolioItem(string userId, AddPortfolioRequest request);

     
    /// Deletes a specific portfolio item.
    Task<resultBase> DeletePortfolioItem(string userId, int itemId);

    // --- Education & Experience ---
    /// Adds educational background (School, Degree, etc.) to the profile.
    Task<resultBase> AddEducation(string userId, AddEducationRequest request);

     
    /// Removes an education record.
    Task<resultBase> DeleteEducation(string userId, int eduId);

     
    /// Adds professional work experience to the profile.
    Task<resultBase> AddExperience(string userId, AddExperienceRequest request);

     
    /// Removes an experience record.
    Task<resultBase> DeleteExperience(string userId, int expId);

    // --- Certifications ---
    /// Adds a professional certificate or license.
    Task<resultBase> AddCertificate(string userId, AddCertificateRequest request);
    Task<resultBase> UpdatePortfolioItem(string userId, int itemId, AddPortfolioRequest request);



}