using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Khdamatk.Server.Contracts.Fawaterak;

/// <summary>
/// Redirection URLs after payment completion
/// </summary>
public class RedirectionUrlsModel
{
    /// <summary>
    /// URL to redirect to on successful payment
    /// </summary>
    [JsonPropertyName("successUrl")]
    [Url]
    public string? OnSuccess { get; set; }

    /// <summary>
    /// URL to redirect to on failed payment
    /// </summary>
    [JsonPropertyName("failUrl")]
    [Url]
    public string? OnFailure { get; set; }

    /// <summary>
    /// URL to redirect to on pending payment
    /// </summary>
    [JsonPropertyName("pendingUrl")]
    [Url]
    public string? OnPending { get; set; }
}
