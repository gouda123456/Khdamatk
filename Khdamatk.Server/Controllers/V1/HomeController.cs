using Khdamatk.Server.Contracts.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class HomeController(IHomeService homeService,IServiceProviderService serviceProviderService) : ControllerBase
{
    private readonly IHomeService homeService = homeService;
    private readonly IServiceProviderService serviceProviderService = serviceProviderService;

    [HttpGet("")]
    public async Task<IActionResult> GetHomeData(CancellationToken cancellationToken)
    {
        var result = await homeService.MainPage(cancellationToken);
        return result.Respond();
    }

    [HttpGet("Freelancers")]
    public async Task<IActionResult> GetFreelancers([FromQuery] FreelancerRequest freelancerRequest,CancellationToken cancellationToken)
    {
        var result = await serviceProviderService.FreelancersPage(freelancerRequest,cancellationToken);
        return result.Respond();
    }


    [HttpGet("freelancer-profile/{userId}")]
    public async Task<IActionResult> GetProfile(string userId, CancellationToken cancellationToken)
    {
        var result = await serviceProviderService.FreelancerProfile(userId, cancellationToken);

        return result.Respond();
    }
    [HttpPut("update-basic-info")]
    public async Task<IActionResult> UpdateInfo(UpdateProfileRequest request)
    => Ok(await serviceProviderService.UpdateProfileBasicInfo(User.GetUserId(), request));

    [HttpPost("portfolio")]
    public async Task<IActionResult> AddWork(AddPortfolioRequest request)
        => Ok(await serviceProviderService.AddPortfolioItem(User.GetUserId(), request));

    [HttpPost("add-education")]
    public async Task<IActionResult> AddEducation([FromBody] AddEducationRequest request)
    {
       
        var userId = User.GetUserId();

        var result = await serviceProviderService.AddEducation(userId, request);

        return Ok(result);
    }
    [HttpPost("add-experience")]
    public async Task<IActionResult> AddExperience([FromBody] AddExperienceRequest request)
    {
        var userId = User.GetUserId(); // دي الـ Extension method اللي بتجيب الـ ID من الـ Token
        var result = await serviceProviderService.AddExperience(userId, request);
        return Ok(result);
    }
    [HttpDelete("portfolio/{itemId}")]
    public async Task<IActionResult> DeletePortfolio(int itemId)
    {
        var userId = User.GetUserId(); // بنجيب الـ ID من الـ Token لضمان الأمان
        var result = await serviceProviderService.DeletePortfolioItem(userId, itemId);

        return result.Respond();
    }
    [HttpPut("update-skills")]
    public async Task<IActionResult> UpdateSkills([FromBody] UpdateSkillsRequest request)
    {
        var userId = User.GetUserId();
        var result = await serviceProviderService.UpdateSkills(userId, request);

        return result.Respond();
    }
}