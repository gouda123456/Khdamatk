using Stripe;

namespace Khdamatk.Server.Helper;

public interface IPaymentHelper
{

    public Task<bool> Pay(string PriceId, string CustomerId);
    public Task<Customer?> AddCustomer(string CustomerId,string UserName, string Email);
    public Task<StripeList<Product>?> GetAllProducts();

    //#region TODO: Checkout Session Creation
    //public Task<string> CreateCheckoutSessionAsync(string serviceName, long amount, string currency, string successUrl, string cancelUrl);
    //public Task<string> CreateCheckoutSessionAsync(string serviceName, long amount, string currency, string successUrl, string cancelUrl, string customerEmail);
    //public Task<string> CreateCheckoutSessionAsync(string serviceName, long amount, string currency, string successUrl, string cancelUrl, string customerEmail, Dictionary<string, string> metadata);
    //public Task<string> CreateCheckoutSessionAsync(string serviceName, long amount, string currency, string successUrl, string cancelUrl, string customerEmail, Dictionary<string, string> metadata, string? couponCode);
    //public Task<string> CreateCheckoutSessionAsync(string serviceName, long amount, string currency, string successUrl, string cancelUrl, string customerEmail, Dictionary<string, string> metadata, string? couponCode, int? trialPeriodDays);
    //public Task<string> CreateCheckoutSessionAsync(string serviceName, long amount, string currency, string successUrl, string cancelUrl, string customerEmail, Dictionary<string, string> metadata, string? couponCode, int? trialPeriodDays, string? customerId);
    //public Task<string> CreateCheckoutSessionAsync(string serviceName, long amount, string currency, string successUrl, string cancelUrl, string customerEmail, Dictionary<string, string> metadata, string? couponCode, int? trialPeriodDays, string? customerId, bool allowPromotionCodes);
    //public Task<string> CreateCheckoutSessionAsync(string serviceName, long amount, string currency, string successUrl, string cancelUrl, string customerEmail, Dictionary<string, string> metadata, string? couponCode, int? trialPeriodDays, string? customerId, bool allowPromotionCodes, string? paymentMethodType);
    //public Task<string> CreateCheckoutSessionAsync(string serviceName, long amount, string currency, string successUrl, string cancelUrl, string customerEmail, Dictionary<string, string> metadata, string? couponCode, int? trialPeriodDays, string? customerId, bool allowPromotionCodes, string? paymentMethodType, Dictionary<string, string>? additionalParams);
    //#endregion
}
