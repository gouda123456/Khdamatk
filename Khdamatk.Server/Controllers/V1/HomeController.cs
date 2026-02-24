using Khdamatk.Server.Contracts.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class HomeController(IHomeService homeService) : ControllerBase
{
    private readonly IHomeService homeService = homeService;

    [HttpGet("")]
    public async Task<IActionResult> GetHomeData(CancellationToken cancellationToken)
    {
        var result = await homeService.MainPage(cancellationToken);
        return result.Respond();
    }

    [HttpGet("Freelancers")]
    public async Task<IActionResult> GetFreelancers([FromQuery] FreelancerRequest freelancerRequest,CancellationToken cancellationToken)
    {
        var result = await homeService.FreelancersPage(freelancerRequest,cancellationToken);
        return result.Respond();
    }


    [HttpGet("freelancer-profile/{userId}")]
    public async Task<IActionResult> GetProfile(string userId, CancellationToken cancellationToken)
    {
        var result = await homeService.FreelancerProfile(userId, cancellationToken);

        return result.Respond();
    }
    [HttpPut("update-basic-info")]
    public async Task<IActionResult> UpdateInfo(UpdateProfileRequest request)
    => Ok(await homeService.UpdateProfileBasicInfo(User.GetUserId(), request));

    [HttpPost("portfolio")]
    public async Task<IActionResult> AddWork(AddPortfolioRequest request)
        => Ok(await homeService.AddPortfolioItem(User.GetUserId(), request));
    [HttpPost("add-education")]
    public async Task<IActionResult> AddEducation([FromBody] AddEducationRequest request)
    {
       
        var userId = User.GetUserId();

        var result = await homeService.AddEducation(userId, request);

        return Ok(result);
    }
    [HttpPost("add-experience")]
    public async Task<IActionResult> AddExperience([FromBody] AddExperienceRequest request)
    {
        var userId = User.GetUserId(); // دي الـ Extension method اللي بتجيب الـ ID من الـ Token
        var result = await homeService.AddExperience(userId, request);
        return Ok(result);
    }
    [HttpDelete("portfolio/{itemId}")]
    public async Task<IActionResult> DeletePortfolio(int itemId)
    {
        var userId = User.GetUserId(); // بنجيب الـ ID من الـ Token لضمان الأمان
        var result = await homeService.DeletePortfolioItem(userId, itemId);

        return result.Respond();
    }
    [HttpPut("update-skills")]
    public async Task<IActionResult> UpdateSkills([FromBody] UpdateSkillsRequest request)
    {
        var userId = User.GetUserId();
        var result = await homeService.UpdateSkills(userId, request);

        return result.Respond();
    }
}