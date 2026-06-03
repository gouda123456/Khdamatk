using Khdamatk.Server.Contracts.Conversations;
using Khdamatk.Server.Contracts.Service;
using Khdamatk.Server.Contracts.WebHook;
using Khdamatk.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Khdamatk.Server.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class ServiceOrderController(IServiceOrderService serviceOrderService) : ControllerBase
{
    private readonly IServiceOrderService serviceOrderService = serviceOrderService;

    #region Service Operations

    [HttpGet("GetServices")]
    [AllowAnonymous]
    public async Task<IActionResult> GetServices([FromQuery] GetServicesRequest request, CancellationToken ct)
    {
        var result = await serviceOrderService.GetServicesAsync(request, ct);
        return Ok(result);
    }

    [HttpGet("GetService/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetService(int id, CancellationToken ct)
    {
        var result = await serviceOrderService.GetServiceAsync(id, ct);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost("AddService")]
    public async Task<IActionResult> AddService([FromBody] AddServiceRequest request, CancellationToken ct)
    {
        var result = await serviceOrderService.AddServiceAsync(request, ct);
        return result.IsSuccess ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    [HttpPut("UpdateService/{id}")]
    public async Task<IActionResult> UpdateService(int id, [FromBody] UpdateServiceRequest request, CancellationToken ct)
    {
        var result = await serviceOrderService.UpdateServiceAsync(id, request, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("DeleteService/{id}")]
    public async Task<IActionResult> DeleteService(int id, CancellationToken ct)
    {
        var result = await serviceOrderService.DeleteServiceAsync(id, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    #endregion

    #region Order Operations

    [HttpPost("add-order/{serviceId}")]
    public async Task<IActionResult> AddOrder(int serviceId, [FromBody] OrderServiceRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await serviceOrderService.AddOrderAsync(serviceId, userId, request, cancellationToken);
        return result.IsSuccess ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    [HttpPut("accept-order/{orderId}")]
    public async Task<IActionResult> AcceptOrder(int orderId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await serviceOrderService.AcceptOrderAsync(orderId, userId, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("reject-order/{orderId}")]
    public async Task<IActionResult> RejectOrder(int orderId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await serviceOrderService.RejectOrderAsync(orderId, userId, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("my-orders")]
    public async Task<IActionResult> GetOrders(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await serviceOrderService.GetOrdersAsync(userId, ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await serviceOrderService.GetOrderAsync(id, userId, ct);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost("{orderId}/pay")]
    public async Task<IActionResult> PayOrder(int orderId, CancellationToken cancellationToken)
    {
        var result = await serviceOrderService.PayOrderAsync(orderId, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{orderId}/complete")]
    public async Task<IActionResult> CompleteOrder(int orderId, [FromBody] ReviewRequest request, CancellationToken cancellationToken)
    {
        var result = await serviceOrderService.CompleteOrderAsync(orderId, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{orderId}/cancel")]
    public async Task<IActionResult> CancelOrder(int orderId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await serviceOrderService.CancelOrderAsync(orderId, userId, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{orderId}/dispute")]
    public async Task<IActionResult> OpenDispute(int orderId, [FromQuery] string reason, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await serviceOrderService.OpenDispute(orderId, userId, reason, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    #endregion

    #region Webhooks

    [HttpPost("webhook/success")]
    [AllowAnonymous]
    public async Task<IActionResult> PaymentSuccess([FromBody] WebHookModel model, CancellationToken ct)
    {
        var result = await serviceOrderService.PaymentSuccessJobOrder(model, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("webhook/failure")]
    [AllowAnonymous]
    public async Task<IActionResult> PaymentFailure([FromBody] CancelTransactionModel model, CancellationToken ct)
    {
        var result = await serviceOrderService.PaymentFailureJobOrder(model, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    #endregion

    #region Conversations

    [HttpGet("conversations")]
    public async Task<IActionResult> GetConversations(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await serviceOrderService.GetConversations(userId, ct);
        return Ok(result);
    }

    [HttpGet("conversations/{orderId}")]
    public async Task<IActionResult> GetConversationMessages(int orderId, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await serviceOrderService.GetConversationMessages(orderId, userId, ct);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost("conversations/{orderId}/submit")]
    public async Task<IActionResult> SubmitWorkAndMessage(int orderId, [FromForm] SubmitWorkAndMessageRequest request, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await serviceOrderService.SubmitWorkAndMessage(orderId, userId, request, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    #endregion
}
