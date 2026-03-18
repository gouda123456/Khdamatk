using Khdamatk.Server.Contracts.Fawaterak;
using Khdamatk.Server.Contracts.Orders;

namespace Khdamatk.Server.Services.Interfaces;

public interface IOrderService : IService
{
    Task HandlePaymentSuccessAsync(long invoiceId, string invoiceKey);
    Task HandlePaymentFailedAsync(long invoiceId, string invoiceKey, string? errorMessage);
    Task HandlePaymentCancelledAsync(string referenceId);

    Task<EInvoiceResponseModel.EInvoiceResponseDataModel?> StartServiceOrderPaymentAsync(int orderId);
    Task<EInvoiceResponseModel.EInvoiceResponseDataModel?> StartJobOrderPaymentAsync(int jobOrderId);

    Task CompleteServiceOrderAsync(int orderId);
    Task OpenDisputeAsync(OrderDisputeRequest request, string currentUserId);
}
