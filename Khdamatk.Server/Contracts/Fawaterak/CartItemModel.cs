using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Khdamatk.Server.Contracts.Fawaterak;

/// <summary>
/// Cart item details
/// </summary>
public class CartItemModel
{
    /// <summary>
    /// Item name
    /// </summary>
    [JsonPropertyName("name")]
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Item price per unit
    /// </summary>
    [JsonPropertyName("price")]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    /// <summary>
    /// Quantity of this item
    /// </summary>
    [JsonPropertyName("quantity")]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
