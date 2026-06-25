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

    [HttpPost("paid_json")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> WebhookPaid([FromBody] WebHookModel model)
    {
        var valid = payments.VerifyWebhook(model);
        if (!valid) return Unauthorized();

        await orderService.HandlePaymentSuccessAsync(model);

        return Ok("got it!");
    }

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