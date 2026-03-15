using Khdamatk.Server.Contracts.Fawaterak;
using Khdamatk.Server.Contracts.Orders;
using Khdamatk.Server.Contracts.WebHook;

namespace Khdamatk.Server.Services.Interfaces;

public interface IOrderService : IService
{
    Task<resultBase> StartServiceOrderPaymentAsync(EInvoiceRequestModel order, string? AdditionalDetails, List<Media> Attachments, int serviceId, string userId);
    public Task<EInvoiceResponseModel.EInvoiceResponseDataModel?> StartJobOrderPaymentAsync(int jobOrderId);


    Task HandlePaymentSuccessAsync(WebHookModel webHookModel);
    Task HandlePaymentFailedAsync(long invoiceId, string invoiceKey, string? errorMessage);
    Task HandlePaymentCancelledAsync(string referenceId);

    

    Task CompleteServiceOrderAsync(int orderId);
    Task OpenDisputeAsync(OrderDisputeRequest request, string currentUserId);
}
