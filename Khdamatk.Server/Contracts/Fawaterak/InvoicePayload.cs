namespace Khdamatk.Server.Contracts.Fawaterak;

/// <summary>
/// Additional payload for the invoice
/// </summary>
public class InvoicePayload
{
    /// <summary>
    /// Your internal order ID
    /// </summary>
    public int OrderId { get; set; }
    public OrderType OrderType { get; set; }

    public ProviderModel Provider { get; set; } = new();
}
public enum OrderType
{
    Service = 1,
    Job = 2
}