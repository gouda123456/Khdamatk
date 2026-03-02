using System.Text.Json.Serialization;
using Newtonsoft.Json;
using static Khdamatk.Server.Contracts.Fawaterak.EInvoiceRequestModel;

namespace Khdamatk.Server.Contracts.Fawaterak;

public class EInvoiceRequestModel 
{

    /// <summary>
    /// Currency code (e.g., EGP, USD)
    /// </summary>
    [JsonPropertyName("currency")]
    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string Currency { get; set; } = "EGP";



    [JsonPropertyName("due_date")]
    public DateTime DueDate { get; set; }

    [JsonProperty("sendEmail")]
    public bool SendEmail { get; set; }

    /// <summary>
    /// Total cart amount (calculated automatically)
    /// </summary>
    [JsonPropertyName("cartTotal")]
    public decimal CartTotal => CartItems.Sum(item => item.Price * item.Quantity);



    /// <summary>
    /// Customer information
    /// </summary>
    [JsonPropertyName("customer")]
    [Required]
    public required CustomerModel Customer { get; set; }


    /// <summary>
    /// List of items in the cart
    /// </summary>
    [JsonPropertyName("cartItems")]
    [MinLength(1)]
    [Required]
    public List<CartItemModel> CartItems { get; set; }


    /// <summary>
    /// Additional payload data
    /// </summary>
    [JsonPropertyName("payLoad")]
    public InvoicePayload? PayLoad { get; set; }


    /// <summary>
    /// URLs for payment result redirections
    /// </summary>
    [JsonPropertyName("redirectionUrls")]
    public RedirectionUrlsModel? RedirectionUrls { get; set; }



    public OrderStatus Status { get; set; } 


}



