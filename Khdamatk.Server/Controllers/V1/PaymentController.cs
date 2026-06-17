using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class PaymentController(IPaymentService paymentService) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetPaymentDetails([FromQuery] string userId)
    {
        var result = await paymentService.GetAllTranactions(userId);
        return Ok(result);
    }
}
