using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers;

[Route("api/[controller]")]
[ApiController]

public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("API is working!");
    }

    [HttpGet]
    [Authorize]
    [Route("authorized")]
    public IActionResult GetAuthorized()
    {
        return Ok($"{HttpContext.User.GetUserId()}You are authorized!");
    }

    [HttpGet]
    [PermissionAuthorize(PermissionsDefault.WeatherForecast.Modify)]
    [Route("permission")]
    public IActionResult Getpermission()
    {
        return Ok($"{HttpContext.User.GetUserId()}You are authorized!");
    }
}
