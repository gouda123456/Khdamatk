using Newtonsoft.Json;

namespace Khdamatk.Server.Contracts.Fawaterak;

public class EInvoiceResponseModel
{

    /// <summary>
    /// Response status
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }

    /// <summary>
    /// Invoice data
    /// </summary>
    [JsonProperty("data")]
    public EInvoiceResponseDataModel Data { get; set; }

    /// <summary>
    /// Invoice response data
    /// </summary>
    public class EInvoiceResponseDataModel
    {
        /// <summary>
        /// Payment URL for the invoice
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Unique invoice ID from Fawaterak
        /// </summary>
        [JsonProperty("invoiceId")]
        public long InvoiceId { get; set; }

        /// <summary>
        /// Invoice key for verification
        /// </summary>
        [JsonProperty("invoiceKey")]
        public string InvoiceKey { get; set; }
    }

}
