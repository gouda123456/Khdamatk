using Asp.Versioning;
using Khdamatk.Server.Contracts.Fawaterak;
using Khdamatk.Server.Helper.Payment;
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

    [HttpGet("send-reset-Password")]
    public IActionResult SendResetEmail([FromServices] IEmailHelper emailHelper)
    {
        emailHelper.SendresetPasswordEmailAsync("giggo343@gmail.com", 666666);
        return Ok();
    }


    [HttpGet("test-Einvoice-Payment")]
    public async Task<IActionResult> TestEInvoicePayment([FromServices] IFawaterakPaymentHelper fawaterakPaymentHelper)
    {


        var response = await fawaterakPaymentHelper.CreateEInvoiceAsync(new EInvoiceRequestModel
        {
            Customer = new CustomerModel
            {
                FirstName = "Gouda",
                LastName = "George",
                CustomerId = "123456",
                Email = "giggo343@gmail.com"
            },
            CartItems = new List<CartItemModel>
            {
                new CartItemModel
                {
                    Name = "Product 1",
                    Quantity = 2,
                    Price = 50
                },
                new CartItemModel
                {
                    Name = "Product 2",
                    Quantity = 1,
                    Price = 100
                }
            },
            Currency = "EGP",
            SendEmail = true,
            RedirectionUrls = new RedirectionUrlsModel()
            {
                OnFailure = "https://www.facebook.com",
                OnPending = "https://www.w3schools.com/cs/cs_math.php",
                OnSuccess = "https://learn.microsoft.com/ar-sa/aspnet/core/?view=aspnetcore-8.0&utm_source=aspnet-start-page&utm_campaign=vside"
            },
            Status = OrderStatus.PendingPayment,
            DueDate = DateTime.UtcNow.AddDays(7),
            PayLoad = new InvoicePayload
            {
                OrderId = 1,
                OrderType = OrderType.Service,
                Provider = new ProviderModel
                {
                    Id = "654321",
                    Username = "Provider Name",
                    Email = "godegeorge07@gmail.com"
                }
            }
        });

        return Ok(response);
    }

    
}