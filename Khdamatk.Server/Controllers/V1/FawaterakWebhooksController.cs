using Khdamatk.Server.Contracts.WebHook;
using Khdamatk.Server.Helper.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Controllers.V1;

/// <summary>
/// Webhook endpoints for Fawaterak payment notifications
/// </summary>
[AllowAnonymous]
[ApiController]
[Route("api/fawaterak/webhooks")]
[Consumes("application/json")]
[Produces("application/json")]
public class FawaterakWebhooksController(
    IFawaterakPaymentHelper payments,
    IOrderService orderService,
    IEmailHelper emailHelper) : ControllerBase
{
    private readonly IFawaterakPaymentHelper payments = payments;
    private readonly IOrderService orderService = orderService;
    private readonly IEmailHelper emailHelper = emailHelper;


    /// <summary>
    /// Handle successful payment notification from Fawaterak
    /// </summary>
    /// <param name="model">Payment webhook data with invoice details and verification hash</param>
    /// <returns>Confirmation message</returns>
    /// <response code="200">Webhook processed successfully</response>
    /// <response code="401">Invalid webhook signature</response>
    [HttpPost("paid_json")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> WebhookPaid([FromBody] WebHookModel model)
    {
        var valid = payments.VerifyWebhook(model);
        if (!valid) return Unauthorized();

        await orderService.HandlePaymentSuccessAsync(model.InvoiceId, model.InvoiceKey);

        return Ok("got it!");
    }



    /// <summary>
    /// Handle payment cancellation notification from Fawaterak
    /// </summary>
    /// <param name="model">Cancellation webhook data with reference ID and verification hash</param>
    /// <returns>Acknowledgment of cancellation</returns>
    /// <response code="200">Cancellation webhook processed successfully</response>
    /// <response code="401">Invalid webhook signature</response>
    [HttpPost("cancel_json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> WebhookCancel([FromBody] CancelTransactionModel model)
    {
        var valid = payments.VerifyCancelTransaction(model);
        if (!valid) return Unauthorized();

        await orderService.HandlePaymentCancelledAsync(model.ReferenceId);

        return Ok();
    }


    /// <summary>
    /// Handle failed payment notification from Fawaterak
    /// </summary>
    /// <param name="model">Failed payment webhook data with invoice details and verification hash</param>
    /// <returns>Acknowledgment of failure</returns>
    /// <response code="200">Failure webhook processed successfully</response>
    /// <response code="401">Invalid webhook signature</response>
    [HttpPost("failed_json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> WebhookFaild([FromBody] FailedWebhookModel model)
    {
        var valid = payments.VerifyFailedWebhook(model);
        if (!valid) return Unauthorized();

        await orderService.HandlePaymentFailedAsync(model.InvoiceId, model.InvoiceKey, model.ErrorMessage);

        return Ok("failed webhook received");
    }


}