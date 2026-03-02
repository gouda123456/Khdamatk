using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Khdamatk.Server.Contracts.Fawaterak;

/// <summary>
/// Customer information
/// </summary>
public class CustomerModel
{
    /// <summary>
    /// Unique customer identifier in your system
    /// </summary>
    [JsonPropertyName("customer_unique_id")]
    public string? CustomerId { get; set; }

    /// <summary>
    /// Customer's first name
    /// </summary>
    [JsonPropertyName("first_name")]
    [Required]
    public required string FirstName { get; set; }

    /// <summary>
    /// Customer's last name
    /// </summary>
    [JsonPropertyName("last_name")]
    [Required]
    public required string LastName { get; set; }

    /// <summary>
    /// Customer's email address
    /// </summary>
    [JsonPropertyName("email")]
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>
    /// Customer's phone number
    /// </summary>
    [JsonPropertyName("phone")]
    [Phone]
    public string? Phone { get; set; }
}
