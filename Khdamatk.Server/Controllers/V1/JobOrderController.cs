using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class JobOrderController(IJobOrderService jobOrderService) : ControllerBase
{
    private readonly IJobOrderService jobOrderService = jobOrderService;

    #region Jobs Operations

    [HttpPost("Jobs/")]
    public async Task<IActionResult> AddJob(AddJobRequest request, CancellationToken cancellationToken)
    {
        return (await jobOrderService.AddJobASync(request, cancellationToken)).Respond();
    }


    [HttpPost("Jobs/{jobId}/Offers/")]
    public async Task<IActionResult> AddOffer([FromRoute] int jobId, [FromForm] AddJopOfferRequest request, CancellationToken cancellationToken)
    {
        return (await jobOrderService.AddOfferAsync(jobId, request, cancellationToken)).Respond();
    }


    [HttpGet("jobs/{jobId}/Offers/")]
    public async Task<IActionResult> ShowOffers([FromRoute] int jobId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.ShowOffersJob(jobId, cancellationToken)).Respond();
    }

    [HttpGet("Jobs/{jobId}/Offer/{offerId}")]
    public async Task<IActionResult> GetOffer([FromRoute] int jobId, [FromRoute] int offerId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.ViewOfferDetails(jobId, offerId, cancellationToken)).Respond();
    }


    #endregion


    #region initialize order


    [HttpPut("Jobs/{jobId}/Offers/{offerId}/Start/")]
    public async Task<IActionResult> AddJob([FromRoute] int jobId, [FromRoute] int offerId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.StartJobOrder(jobId, offerId, cancellationToken)).Respond();
    }

    [HttpPut("Jobs/{jobId}/Offers/{offerId}/ChangeSelectionTo/{newOfferId}/")]
    public async Task<IActionResult> ChangeSelectionOfferJob([FromRoute] int jobId, [FromRoute] int offerId, [FromRoute] int newOfferId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.ChangeSelectionOfferJob(jobId, offerId, newOfferId, User?.GetUserId()!, cancellationToken)).Respond();
    }


    [HttpPut("Jobs/{jobId}/Offers/{offerId}/reject/")]
    public async Task<IActionResult> RejectOffer([FromRoute] int jobId, [FromRoute] int offerId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.RejectOfferJob(jobId, offerId, cancellationToken)).Respond();
    }




    #endregion

    #region Order middle Operations


    [HttpPut("JobOrders/{jobId}/Cancel/")]
    public async Task<IActionResult> CancelJobOrder([FromRoute] int jobId, [FromQuery] string userId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.CancelJobOrder(jobId, userId, cancellationToken)).Respond();
    }



    [HttpGet("JobOrders/{orderId}/Summary/")]
    public async Task<IActionResult> OrderSummary([FromRoute] int orderId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.OrderSummary(orderId, User.GetUserId()!)).Respond();
    }

    [HttpGet("JobOrders/{orderId}/")]
    public async Task<IActionResult> GetOrder([FromRoute] int orderId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.OrderDetails(orderId, User.GetUserId()!)).Respond();
    }

    [HttpPost("JobOrders/{orderId}/SubmitWorkAndMessage/")]
    public async Task<IActionResult> SubmitWorkAndMessage([FromRoute] int orderId, [FromBody] SubmitWorkAndMessageRequest request, CancellationToken cancellationToken)
    {
        return (await jobOrderService.SubmitWorkAndMessage(orderId, User.GetUserId()!, request, cancellationToken)).Respond();
    }

    [HttpGet("JobOrders/{orderId}/ConversationMessages/")]
    public async Task<IActionResult> GetConversationMessages([FromRoute] int orderId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.GetConversationMessages(orderId, User.GetUserId()!, cancellationToken)).Respond();
    }

    [HttpGet("JobOrders/Conversations/")]
    public async Task<IActionResult> GetConversations(CancellationToken cancellationToken)
    {
        return (await jobOrderService.GetConversations(User.GetUserId()!, cancellationToken)).Respond();
    }

    #endregion


    #region Order End Operations


    [HttpPut("JobOrders/{orderId}/Complete/")]
    public async Task<IActionResult> CompleteJobOrder([FromRoute] int orderId, [FromBody] ReviewRequest request, CancellationToken cancellationToken)
    {
        return (await jobOrderService.CompleteJobOrder(orderId, request, cancellationToken)).Respond();
    }

    [HttpPost("JobOrders/{orderId}/OpenDispute/")]
    public async Task<IActionResult> OpenDispute([FromRoute] int orderId, [FromBody] string reasonDetails, CancellationToken cancellationToken)
    {
        return (await jobOrderService.OpenDispute(orderId, User.GetUserId()!, reasonDetails, cancellationToken)).Respond();
    }




    #endregion

    [HttpPost("add-order")]
    public async Task<IActionResult> AddOrder([FromBody] CreateJobOrderRequest request, CancellationToken cancellationToken)
    {
        // 1. لازم تجيب الـ UserId بتاع الشخص اللي عامل Login حالياً (العميل)
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        // 2. ابعت الـ userId للـ Service لأنها مستنياه في الترتيب التاني
        var result = await jobOrderService.AddOrderAsync(request, userId, cancellationToken);

        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, result)
            : BadRequest(result);
    }

    // 2. قبول الأوردر (من طرف الفريلانسر)
    [HttpPut("accept-order/{orderId}")]
    public async Task<IActionResult> AcceptOrder(int orderId, CancellationToken cancellationToken)
    {
        // هنا بنجيب الـ UserId بتاع الشخص اللي عامل Login حالياً (الفريلانسر)
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await jobOrderService.AcceptOrderAsync(orderId, userId, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    // 3. رفض الأوردر (من طرف الفريلانسر)
    [HttpPut("reject-order/{orderId}")]
    public async Task<IActionResult> RejectOrder(int orderId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await jobOrderService.RejectOrderAsync(orderId, userId, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);








    }
}

