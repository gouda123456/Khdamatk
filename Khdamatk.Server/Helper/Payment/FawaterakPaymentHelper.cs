using System.Buffers.Text;
using System.Text.Json;
using Khdamatk.Server.Contracts.Fawaterak;
using Khdamatk.Server.Contracts.WebHook;
using static Khdamatk.Server.Contracts.Fawaterak.EInvoiceResponseModel;

namespace Khdamatk.Server.Helper.Payment;

public class FawaterakPaymentHelper : IFawaterakPaymentHelper
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly string ApiKey;
    private readonly string BaseUrl;
    private readonly string ProviderKey;
    private readonly string CreateEInvoiceEndpoint;

    public FawaterakPaymentHelper(IHttpClientFactory httpClientFactory, IOptions<FawaterakSettings> options)
    {
        this.httpClientFactory = httpClientFactory;
        var FawaterakSettings = options.Value;
        ApiKey = FawaterakSettings.ApiKey;
        BaseUrl = FawaterakSettings.BaseUrl;
        ProviderKey = FawaterakSettings.ProviderKey;

        CreateEInvoiceEndpoint = $"{BaseUrl}/createInvoiceLink";
    }


    public async Task<EInvoiceResponseDataModel?> CreateEInvoiceAsync(EInvoiceRequestModel eInvoice)
    {
        // 1. استخراج سعر الخدمة الأساسي قبل إضافة العمولة
        decimal serviceOriginalPrice = eInvoice.CartItems.Sum(x => x.Price * x.Quantity);

        // 2. حساب عمولة الموقع (مثال: 10%)
        decimal platformFee = serviceOriginalPrice * 0.10m + 10;


        eInvoice.CartItems.Add(new CartItemModel 
        { 
            Name = "خدماتك",
            Quantity = 1,
            Price = platformFee
        });

        eInvoice.DueDate = DateTime.UtcNow.AddDays(5); // Set due date to 7 days from now

        // 1. إعداد خيارات الـ JSON لتناسب الـ API
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // هذا هو مفتاح الحل
            WriteIndented = true
        };

        // 2. تحويل الكائن إلى JSON باستخدام الخيارات
        var jsonPayload = JsonSerializer.Serialize(eInvoice, jsonOptions);


        var client = httpClientFactory.CreateClient();
        var reruest = new HttpRequestMessage(HttpMethod.Post, CreateEInvoiceEndpoint);
        reruest.Headers.Add("Authorization", $"Bearer {ApiKey}");
        reruest.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await client.SendAsync(reruest);

        if(response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var eInvoiceResponse = JsonSerializer.Deserialize<EInvoiceResponseModel>(responseContent);

            return eInvoiceResponse?.Data;
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            // ضع Breakpoint هنا وشوف قيمة errorContent
            // غالباً هتلاقي رسالة بتقول "The amount field is required" أو "Cart total mismatch"
            throw new Exception($"Fawaterak Error: {errorContent}");
        }

        return null;

    }


    #region WebHook Verification
    public bool VerifyWebhook(WebHookModel webHook)
    {
        var generatedHashKey =
            GenerateHashKeyForWebhookVerification(webHook.InvoiceId, webHook.InvoiceKey, webHook.PaymentMethod);
        return generatedHashKey == webHook.HashKey;
    }

    public bool VerifyCancelTransaction(CancelTransactionModel cancelTransaction)
    {
        var generatedHashKey = GenerateHashKeyForCancelTransaction(cancelTransaction.ReferenceId, cancelTransaction.PaymentMethod);
        return generatedHashKey == cancelTransaction.HashKey;
    }

    public bool VerifyFailedWebhook(FailedWebhookModel failedWebhook)
    {
        if (string.IsNullOrWhiteSpace(failedWebhook.HashKey))
            return false;

        var generatedHashKey = GenerateHashKeyForFailedWebhook(
            failedWebhook.InvoiceId,
            failedWebhook.InvoiceKey,
            failedWebhook.PaymentMethod);

        return string.Equals(generatedHashKey, failedWebhook.HashKey, StringComparison.OrdinalIgnoreCase);
    }

    public bool VerifyApiKeyTransaction(string apiKey)
    {
        return apiKey == ApiKey;
    }

    #endregion


    #region Generate HashKey
    public string GenerateHashKeyForIFrame(string domain)
    {
        var queryParam = $"Domain={domain}&ProviderKey={ProviderKey}";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(ApiKey));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(queryParam));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    private string GenerateHashKeyForWebhookVerification(long invoiceId, string invoiceKey, string paymentMethod)
    {
        var queryParam = $"InvoiceId={invoiceId}&InvoiceKey={invoiceKey}&PaymentMethod={paymentMethod}";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(ApiKey));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(queryParam));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    private string GenerateHashKeyForCancelTransaction(string referenceId, string paymentMethod)
    {
        var queryParam = $"referenceId={referenceId}&PaymentMethod={paymentMethod}";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(ApiKey));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(queryParam));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    // Failed webhook uses the vendor key (ProviderKey) per Fawaterak docs:
    // "InvoiceId=response.invoice_id&InvoiceKey=response.invoice_key&PaymentMethod=response.payment_method"
    private string GenerateHashKeyForFailedWebhook(long invoiceId, string invoiceKey, string paymentMethod)
    {
        var queryParam = $"InvoiceId={invoiceId}&InvoiceKey={invoiceKey}&PaymentMethod={paymentMethod}";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(ProviderKey));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(queryParam));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }

    #endregion
}
