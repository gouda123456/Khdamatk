using Khdamatk.Server.Contracts.Fawaterak;
using Khdamatk.Server.Contracts.orders;
using Khdamatk.Server.Contracts.Orders;
using Khdamatk.Server.Contracts.WebHook;

namespace Khdamatk.Server.Services.Interfaces;

public interface IOrderService : IService
{
    Task<resultBase> StartServiceOrderPaymentAsync(StartServiceOrderPaymentRequest request,string userId);
    public Task<EInvoiceResponseModel.EInvoiceResponseDataModel?> StartJobOrderPaymentAsync(int jobOrderId);


    Task HandlePaymentSuccessAsync(WebHookModel webHookModel);
    Task HandlePaymentFailedAsync(long invoiceId, string invoiceKey, string? errorMessage);
    Task HandlePaymentCancelledAsync(string referenceId);

    

    Task CompleteServiceOrderAsync(int orderId);
    Task OpenDisputeAsync(OrderDisputeRequest request, string currentUserId);
}

/*
 * Create Base order
 * {
 * - create order + payment Start
 * -payment Success
 * -payment Failed 
 * -order.state = in progress + send email to free lancer and customer 
 * - submit work + message + both
 * - complete order + send email to free lancer and customer
 * - open dispute + send email to free lancer and customer
 * }
 * 
 * Create Job Order
 * {
 * -add Job post :Done
 * -post Job offer : Done
 * -select job offer
 * -create order + payment start 
 * -success payment + change order.state = in progress + send email to free lancer and customer
 * - payment Failed + cancel order + send email to customer
 * -submit work + message + both
 * -complete order + send email to free lancer
 * - open dispute + send email to the other party
 * }
 * 
 * Create Service Order
 * {
 * - select service + Create order +  send email to free lancer
 * - free lancer response to order + send email to customer
 * - if(lancer.response == true ) => payment start + send email to customer
 * - if(lancer.response == false ) => cancel order + send email to customer
 * - payment Success + change order.state = in progress + send email to free lancer and customer
 * - payment Failed + cancel order + send email to customer
 * - submit work + message + both
 * - complete order + send email to free lancer
 * - open dispute + send email to the other party
 */



