using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Climate;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class JobOrderController(IJobOrderService jobOrderService) : ControllerBase
{
    private readonly IJobOrderService jobOrderService = jobOrderService;

    #region Jobs Operations

    [HttpPost("Jobs/")]
    [Authorize(Roles = $"{RolesStrings.Admin},{RolesStrings.Member}")]
    public async Task<IActionResult> AddJob([FromForm]AddJobRequest request, CancellationToken cancellationToken)
    {
        return (await jobOrderService.AddJobAsync(request, cancellationToken)).Respond();
    }


    [HttpPost("Jobs/{jobId}/Offers")]
    [Authorize(Roles = $"{RolesStrings.Admin},{RolesStrings.ServiceProvider}")]
    public async Task<IActionResult> AddOffer([FromRoute] int jobId, [FromForm] AddJopOfferRequest request, CancellationToken cancellationToken)
    {
        return (await jobOrderService.AddOfferAsync(jobId, request, cancellationToken)).Respond();
    }


    [HttpGet("jobs/{jobId}/Offers")]
    [Authorize(Roles = $"{RolesStrings.Admin},{RolesStrings.Member}")]
    public async Task<IActionResult> ShowOffers([FromRoute] int jobId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.ShowOffersJob(jobId, cancellationToken)).Respond();
    }

    [HttpGet("Jobs/{jobId}/Offer/{offerId}")]
    [Authorize(Roles = $"{RolesStrings.Admin},{RolesStrings.Member}")]
    public async Task<IActionResult> GetOffer([FromRoute] int jobId, [FromRoute] int offerId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.ViewOfferDetails(jobId, offerId, cancellationToken)).Respond();
    }


    #endregion


    #region initialize order


    [HttpPut("Jobs/{jobId}/Offers/{offerId}/Start/")]
    [Authorize(Roles = $"{RolesStrings.Admin},{RolesStrings.Member}")]
    public async Task<IActionResult> StartJob([FromRoute] int jobId, [FromRoute] int offerId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.StartJobOrder(jobId, offerId, cancellationToken)).Respond();
    }

    [HttpPut("Jobs/{jobId}/Offers/{offerId}/ChangeSelectionTo/{newOfferId}/")]
    [Authorize(Roles = $"{RolesStrings.Admin},{RolesStrings.Member}")]
    public async Task<IActionResult> ChangeSelectionOfferJob([FromRoute] int jobId, [FromRoute] int offerId, [FromRoute] int newOfferId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.ChangeSelectionOfferJob(jobId, offerId, newOfferId, User?.GetUserId()!, cancellationToken)).Respond();
    }


    [HttpPut("Jobs/{jobId}/Offers/{offerId}/reject/")]
    [Authorize(Roles = $"{RolesStrings.Admin},{RolesStrings.Member}")]
    public async Task<IActionResult> RejectOffer([FromRoute] int jobId, [FromRoute] int offerId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.RejectOfferJob(jobId, offerId, cancellationToken)).Respond();
    }




    #endregion

    #region Order middle Operations


    [HttpPut("JobOrders/{jobId}/Cancel/")]
    [Authorize(Roles = $"{RolesStrings.Admin},{RolesStrings.Member}")]
    public async Task<IActionResult> CancelJobOrder([FromRoute] int jobId, [FromQuery] string userId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.CancelJobOrder(jobId, userId, cancellationToken)).Respond();
    }



    [HttpGet("JobOrders/{orderId}/Summary/")]
    [Authorize(Roles = $"{RolesStrings.Admin},{RolesStrings.Member},{RolesStrings.ServiceProvider}")]
    public async Task<IActionResult> OrderSummary([FromRoute] int orderId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.OrderSummary(orderId, User.GetUserId()!)).Respond();
    }

    [HttpGet("JobOrders/{orderId}/")]
    [Authorize(Roles = $"{RolesStrings.Admin},{RolesStrings.Member},{RolesStrings.ServiceProvider}")]
    public async Task<IActionResult> GetOrder([FromRoute] int orderId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.OrderDetails(orderId, User.GetUserId()!)).Respond();
    }

    [HttpPost("JobOrders/{orderId}/SubmitWorkAndMessage/")]
    [Authorize(Roles = $"{RolesStrings.Admin},{RolesStrings.Member},{RolesStrings.ServiceProvider}")]
    public async Task<IActionResult> SubmitWorkAndMessage([FromRoute] int orderId, [FromForm] SubmitWorkAndMessageRequest request, CancellationToken cancellationToken)
    {
        return (await jobOrderService.SubmitWorkAndMessage(orderId, User.GetUserId()!, request, cancellationToken)).Respond();
    }

    [HttpGet("JobOrders/{orderId}/ConversationMessages/")]
    [Authorize(Roles = $"{RolesStrings.Admin},{RolesStrings.Member},{RolesStrings.ServiceProvider}")]
    public async Task<IActionResult> GetConversationMessages([FromRoute] int orderId, CancellationToken cancellationToken)
    {
        return (await jobOrderService.GetConversationMessages(orderId, User.GetUserId()!, cancellationToken)).Respond();
    }

    [HttpGet("JobOrders/Conversations/")]
    [Authorize(Roles = $"{RolesStrings.Admin},{RolesStrings.Member},{RolesStrings.ServiceProvider}")]
    public async Task<IActionResult> GetConversations(CancellationToken cancellationToken)
    {
        return (await jobOrderService.GetConversations(User.GetUserId()!, cancellationToken)).Respond();
    }

    #endregion


    #region Order End Operations


    [HttpPut("JobOrders/{orderId}/Complete/")]
    [Authorize]
    public async Task<IActionResult> CompleteJobOrder([FromRoute] int orderId, [FromBody] ReviewRequest request, CancellationToken cancellationToken)
    {
        return (await jobOrderService.CompleteJobOrder(orderId, request, cancellationToken)).Respond();
    }

    [HttpPost("JobOrders/{orderId}/OpenDispute/")]
    [Authorize]
    public async Task<IActionResult> OpenDispute([FromRoute] int orderId, [FromBody] string reasonDetails, CancellationToken cancellationToken)
    {
        return (await jobOrderService.OpenDispute(orderId, User.GetUserId()!, reasonDetails, cancellationToken)).Respond();
    }
}




    #endregion



