using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.Contracts.WebHook;

/// <summary>
/// Webhook payload data
/// </summary>
public class WebhookPayload
{
    /// <summary>
    /// Order ID from your system
    /// </summary>
    [JsonPropertyName("OrderId")]
    public string? OrderId { get; set; }
}

/// <summary>
/// Webhook model for successful payment notifications
/// </summary>
public class WebHookModel
{
    /// <summary>
    /// Invoice ID from Fawaterak
    /// </summary>
    [JsonPropertyName("invoice_id")]
    [Required]
    public long InvoiceId { get; set; }

    /// <summary>
    /// Invoice key from Fawaterak
    /// </summary>
    [JsonPropertyName("invoice_key")]
    [Required]
    public string InvoiceKey { get; set; }

    /// <summary>
    /// Verification hash key
    /// </summary>
    [JsonPropertyName("hashKey")]
    [Required]
    public string HashKey { get; set; }

    /// <summary>
    /// Payment method used for the transaction
    /// </summary>
    [JsonPropertyName("payment_method")]
    [Required]
    public string PaymentMethod { get; set; }

    /// <summary>
    /// Current status of the invoice
    /// </summary>
    [JsonPropertyName("invoice_status")]
    [Required]
    public string InvoiceStatus { get; set; }

    /// <summary>
    /// Payload as JSON string
    /// </summary>
    [JsonPropertyName("pay_load")]
    public string? PayloadString { get; set; }

    /// <summary>
    /// Parsed payload data
    /// </summary>
    //[JsonPropertyName("pay_load")]
    public WebhookPayload? Payload { get; set; }
}

/// <summary>
/// Webhook model for cancelled or failed transactions
/// </summary>
public class CancelTransactionModel
{
    /// <summary>
    /// Verification hash key
    /// </summary>
    [JsonPropertyName("hashKey")]
    [Required]
    public string HashKey { get; set; }

    /// <summary>
    /// Transaction reference ID
    /// </summary>
    [JsonPropertyName("referenceId")]
    [Required]
    public string ReferenceId { get; set; }

    /// <summary>
    /// Transaction status
    /// </summary>
    [JsonPropertyName("status")]
    [Required]
    public string Status { get; set; }

    /// <summary>
    /// Payment method used for the transaction
    /// </summary>
    [JsonPropertyName("paymentMethod")]
    [Required]
    public string PaymentMethod { get; set; }

    /// <summary>
    /// Additional payload data
    /// </summary>
    [JsonPropertyName("pay_load")]
    public object? PayLoad { get; set; }
}

/// <summary>
/// Failed webhook response details object
/// </summary>
public class FailedWebhookResponse
{
    [JsonPropertyName("gatewayCode")]
    public string? GatewayCode { get; set; }

    [JsonPropertyName("gatewayRecommendation")]
    public string? GatewayRecommendation { get; set; }
}

/// <summary>
/// Failed webhook model (JSON body)
/// Matches the example failed webhook payload from Fawaterak.
/// </summary>
public class FailedWebhookModel
{
    [JsonPropertyName("invoice_key")]
    [Required]
    public string InvoiceKey { get; set; }

    [JsonPropertyName("invoice_id")]
    [Required]
    public long InvoiceId { get; set; }

    [JsonPropertyName("payment_method")]
    [Required]
    public string PaymentMethod { get; set; }

    // Hash key sent by Fawaterak to verify the payload (if provided)
    [JsonPropertyName("hashKey")]
    public string? HashKey { get; set; }

    [JsonPropertyName("pay_load")]
    public object? Payload { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("paidCurrency")]
    public string? PaidCurrency { get; set; }

    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }

    [JsonPropertyName("response")]
    public FailedWebhookResponse? Response { get; set; }

    [JsonPropertyName("referenceNumber")]
    public string? ReferenceNumber { get; set; }
}
