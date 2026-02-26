using Khdamatk.Server.Contracts.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class ServiceProviderController(IServiceProviderService ServiceProviderService) : ControllerBase
{
    private readonly IServiceProviderService ServiceProviderService = ServiceProviderService;

    [HttpGet("Freelancers")]
    public async Task<IActionResult> GetFreelancers([FromQuery] FreelancerRequest freelancerRequest, CancellationToken cancellationToken)
    {
        var result = await ServiceProviderService.FreelancersPage(freelancerRequest, cancellationToken);
        return result.Respond();
    }

    [HttpGet("freelancer-profile/{userId}")]
    public async Task<IActionResult> GetProfile(string userId, CancellationToken cancellationToken)
    {
        var result = await ServiceProviderService.FreelancerProfile(userId, cancellationToken);
        return result.Respond();
    }

    [HttpPut("update-basic-info")]
    public async Task<IActionResult> UpdateInfo(UpdateProfileRequest request)
    {
        var result = await ServiceProviderService.UpdateProfileBasicInfo(User.GetUserId(), request);
        return result.Respond();
    }

    [HttpPost("portfolio")]
    public async Task<IActionResult> AddWork(AddPortfolioRequest request)
    {
        var result = await ServiceProviderService.AddPortfolioItem(User.GetUserId(), request);
        return result.Respond();
    }

    [HttpPost("add-education")]
    public async Task<IActionResult> AddEducation([FromBody] AddEducationRequest request)
    {
        var userId = User.GetUserId();
        var result = await ServiceProviderService.AddEducation(userId, request);
        return result.Respond();
    }

    [HttpPost("add-experience")]
    public async Task<IActionResult> AddExperience([FromBody] AddExperienceRequest request)
    {
        var userId = User.GetUserId();
        var result = await ServiceProviderService.AddExperience(userId, request);
        return result.Respond();
    }

    [HttpDelete("portfolio/{itemId}")]
    public async Task<IActionResult> DeletePortfolio(int itemId)
    {
        var userId = User.GetUserId();
        var result = await ServiceProviderService.DeletePortfolioItem(userId, itemId);
        return result.Respond();
    }

    [HttpPut("update-skills")]
    public async Task<IActionResult> UpdateSkills([FromBody] UpdateSkillsRequest request)
    {
        var userId = User.GetUserId();
        var result = await ServiceProviderService.UpdateSkills(userId, request);
        return result.Respond();
    }
}