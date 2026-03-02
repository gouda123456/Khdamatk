using Khdamatk.Server.Contracts.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;


/// Controller for managing service provider operations including profile browsing, 
/// updates, and CV management (Portfolio, Education, Experience, Certificates).

[Route("api/[controller]")]
[ApiController]
public class ServiceProviderController(IServiceProviderService _service) : ControllerBase
{
    private readonly IServiceProviderService _service = _service;


    /// Retrieves a list of freelancers based on search and filter criteria.

    [HttpGet("Freelancers")]
    public async Task<IActionResult> GetFreelancers([FromQuery] FreelancerRequest? freelancerRequest, CancellationToken cancellationToken)
    {
        var result = await _service.FreelancersPage(freelancerRequest, cancellationToken);
        return result.Respond();
    }


    /// Fetches the detailed profile of a specific freelancer.
    [HttpGet("freelancer-profile/{userId}")]
    public async Task<IActionResult> GetProfile(string userId, CancellationToken cancellationToken)
    {
        var result = await _service.FreelancerProfile(userId, cancellationToken);
        return result.Respond();
    }

    /// Updates basic profile information (Title, Bio, Hourly Rate, Social Links).
    [HttpPut("update-basic-info")]
    public async Task<IActionResult> UpdateInfo(UpdateProfileRequest request)
    {
        var result = await _service.UpdateProfileBasicInfo(User.GetUserId(), request);
        return result.Respond();
    }

    /// Adds a new project to the provider's portfolio.
    [HttpPost("portfolio")]
    public async Task<IActionResult> AddWork(AddPortfolioRequest request)
    {
        var result = await _service.AddPortfolioItem(User.GetUserId(), request);
        return result.Respond();
    }


    /// Deletes a specific portfolio item by ID.
    [HttpDelete("portfolio/{itemId}")]
    public async Task<IActionResult> DeletePortfolio(int itemId)
    {
        var result = await _service.DeletePortfolioItem(User.GetUserId(), itemId);
        return result.Respond();
    }


    /// Adds educational background to the profile.
    [HttpPost("add-education")]
    public async Task<IActionResult> AddEducation([FromBody] AddEducationRequest request)
    {
        var result = await _service.AddEducation(User.GetUserId(), request);
        return result.Respond();
    }


    /// Removes an education record from the profile.

    [HttpDelete("education/{id}")]
    public async Task<IActionResult> DeleteEducation(int id)
    {
        var result = await _service.DeleteEducation(User.GetUserId(), id);
        return result.Respond();
    }


    /// Adds work experience to the profile.

    [HttpPost("add-experience")]
    public async Task<IActionResult> AddExperience([FromBody] AddExperienceRequest request)
    {
        var result = await _service.AddExperience(User.GetUserId(), request);
        return result.Respond();
    }


    /// Removes an experience record from the profile.
    [HttpDelete("experience/{id}")]
    public async Task<IActionResult> DeleteExperience(int id)
    {
        var result = await _service.DeleteExperience(User.GetUserId(), id);
        return result.Respond();
    }


    /// Updates the list of skills for the provider.
    [HttpPut("update-skills")]
    public async Task<IActionResult> UpdateSkills([FromBody] UpdateSkillsRequest request)
    {
        var result = await _service.UpdateSkills(User.GetUserId(), request);
        return result.Respond();
    }


    /// Adds a professional certificate to the profile.
    [HttpPost("add-certificate")]
    public async Task<IActionResult> AddCertificate([FromBody] AddCertificateRequest request)
    {
        var result = await _service.AddCertificate(User.GetUserId(), request);
        return result.Respond();
    }

    [HttpPut("portfolio/{itemId}")]
    public async Task<IActionResult> UpdatePortfolio(int itemId, [FromBody] AddPortfolioRequest request)
    {
        var result = await _service.UpdatePortfolioItem(User.GetUserId(), itemId, request);
        return result.Respond();
    }
}