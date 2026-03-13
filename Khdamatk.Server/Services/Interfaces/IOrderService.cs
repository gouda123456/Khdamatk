using Khdamatk.Server.Contracts.Fawaterak;
using Khdamatk.Server.Contracts.Orders;

namespace Khdamatk.Server.Services.Interfaces;

public interface IOrderService : IService
{
    Task<resultBase> StartServiceOrderPaymentAsync(EInvoiceRequestModel order, string? AdditionalDetails, List<Media> Attachments, int serviceId, string userId);
    Task<EInvoiceResponseModel.EInvoiceResponseDataModel?> StartJobOrderPaymentAsync(EInvoiceRequestModel order);


    Task HandlePaymentSuccessAsync(long invoiceId, string invoiceKey);
    Task HandlePaymentFailedAsync(long invoiceId, string invoiceKey, string? errorMessage);
    Task HandlePaymentCancelledAsync(string referenceId);

    

    Task CompleteServiceOrderAsync(int orderId);
    Task OpenDisputeAsync(OrderDisputeRequest request, string currentUserId);
}
