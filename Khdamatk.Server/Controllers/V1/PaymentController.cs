using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class PaymentController(IPaymentService paymentService) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetPaymentDetails()
    {
        var result = await paymentService.GetAllTranactions(User.GetUserId()!);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("PayToWallet")]
    public async Task<IActionResult> PayToWallet( [FromBody] PayToWalletRequest request)
    {
        var result = await paymentService.PayToWallet(request, User.GetUserId()!);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("Witherdraw")]
    public async Task<IActionResult> Witherdraw([FromBody] PayToWalletRequest request)
    {
        var result = await paymentService.Witherdraw(request, User!.GetUserId()!);

        //TODO: 
        return Ok(result);
    }

}
