using Khdamatk.Server.Contracts.Fawaterak;
using Khdamatk.Server.Contracts.WebHook;
using static Khdamatk.Server.Contracts.Fawaterak.EInvoiceResponseModel;

namespace Khdamatk.Server.Helper.Payment;

public interface IFawaterakPaymentHelper
{
    // Create EInvoice Link
    Task<EInvoiceResponseDataModel?> CreateEInvoiceAsync(EInvoiceRequestModel eInvoice);

    // WebHook Verification
    bool VerifyWebhook(WebHookModel webHook);
    bool VerifyCancelTransaction(CancelTransactionModel cancelTransaction);
    bool VerifyFailedWebhook(FailedWebhookModel failedWebhook);
    bool VerifyApiKeyTransaction(string apiKey);

    // HashKey
    string GenerateHashKeyForIFrame(string domain);
}
