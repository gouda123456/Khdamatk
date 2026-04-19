using Khdamatk.Server.Contracts.Service;
using Khdamatk.Server.Contracts.WebHook;

namespace Khdamatk.Server.Services.Interfaces;

public interface IServiceOrderService : IService
{
    /*TODOs:
     * CRUD:
    * {
        * AddService,       S
        * GetService,       S
        * GetServices,      S
        * UpdateService,    S
        * DeleteService     S
    * }
     * 
     * Order:
    * {
        * addOrder,                 A
        * FreeLancerAcceptOrder,    A
        * FreelancerRejectOrder,    A
        * PayOrder,                     G
        * PaymentSuccess,               G
        * PaymentFailure,               G
        * GetOrder,                 A
        * GetOrders,                A
        * SubmitWork(files or message and assign to userId),        G
        * CompleteOrder,        G
        * CancelOrder,          G
        * AriseDispute,         G
    * }
     */

    Task<resultBase> AddServiceAsync(AddServiceRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> GetServiceAsync(int serviceId, CancellationToken cancellationToken = default);
    Task<resultBase> GetServicesAsync(CancellationToken cancellationToken = default);
    Task<resultBase> UpdateServiceAsync(int serviceId, AddServiceRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> DeleteServiceAsync(int serviceId, CancellationToken cancellationToken = default);
    Task<resultBase> AddOrderAsync(int ServiceId,string CustomerId,OrderServiceRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> FreeLancerAcceptOrderAsync(int orderId, CancellationToken cancellationToken = default);
    Task<resultBase> FreelancerRejectOrderAsync(int orderId, CancellationToken cancellationToken = default);
    Task<resultBase> PayOrderAsync(int orderId, CancellationToken cancellationToken = default);
    Task<resultBase> PaymentSuccessJobOrder(WebHookModel model, CancellationToken cancellationToken);
    Task<resultBase> PaymentFailureJobOrder(CancelTransactionModel model, CancellationToken cancellationToken);
    Task<resultBase> GetOrderAsync(int orderId, CancellationToken cancellationToken = default);
    Task<resultBase> GetOrdersAsync(CancellationToken cancellationToken = default);
    Task<resultBase> SubmitWorkAndMessage(int orderId, string userId, SubmitWorkAndMessageRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> GetConversations(string userId, CancellationToken cancellationToken = default);
    Task<resultBase> GetConversationMessages(int orderId, string UserId, CancellationToken cancellationToken = default);
    Task<resultBase> CompleteOrderAsync(int orderId, ReviewRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> CancelOrderAsync(int orderId, CancellationToken cancellationToken = default);
    Task<resultBase> AriseDisputeAsync(int orderId, string reason, CancellationToken cancellationToken = default);


}
