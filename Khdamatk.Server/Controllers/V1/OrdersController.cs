using Khdamatk.Server.Contracts.Fawaterak;
using Khdamatk.Server.Contracts.orders;
using Khdamatk.Server.Contracts.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService orderService = orderService;

    /// <summary>
    /// Start payment for a service order and return the Fawaterak payment URL.
    /// </summary>
    [HttpPost("{orderId:int}/pay-service")]
    public async Task<IActionResult> StartServiceOrderPayment(StartServiceOrderPaymentRequest request,[FromRoute] int orderId)
    {
        if(User?.GetUserId() == null) return Unauthorized();
        
        var result = await orderService.StartServiceOrderPaymentAsync(request, orderId, User.GetUserId()!);
        if (result is null)
            return BadRequest("Unable to start payment for this order.");

        return result.Respond();
    }

    /// <summary>
    /// Start payment for a job order and return the Fawaterak payment URL.
    /// </summary>
    [HttpPost("jobs/{jobOrderId:int}/pay")]
    public async Task<IActionResult> StartJobOrderPayment(int jobOrderId)
    {
        var response = await orderService.StartJobOrderPaymentAsync(jobOrderId);
        if (response is null)
            return BadRequest("Unable to start payment for this job order.");

        return Ok(response);
    }

    /// <summary>
    /// Mark a service order as completed (after both sides confirm).
    /// </summary>
    [HttpPost("{orderId:int}/complete")]
    public async Task<IActionResult> CompleteServiceOrder(int orderId)
    {
        await orderService.CompleteServiceOrderAsync(orderId);
        return Ok();
    }

    /// <summary>
    /// Open a dispute on a service order.
    /// </summary>
    [HttpPost("disputes")]
    public async Task<IActionResult> OpenDispute([FromBody] OrderDisputeRequest request)
    {
        var userId = User.GetUserId();
        await orderService.OpenDisputeAsync(request, userId);
        return Ok();
    }
}

