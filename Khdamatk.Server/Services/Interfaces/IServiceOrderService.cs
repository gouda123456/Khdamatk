using Khdamatk.Server.Contracts.Service;
using Khdamatk.Server.Contracts.WebHook;
using Khdamatk.Server.Contracts.Conversations;

namespace Khdamatk.Server.Services.Interfaces;

public interface IServiceOrderService : IService
{
    // Services
    Task<resultBase> AddServiceAsync(AddServiceRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> GetServiceAsync(int serviceId, CancellationToken cancellationToken = default);
    Task<resultBase> GetServicesAsync(GetServicesRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> UpdateServiceAsync(int serviceId, UpdateServiceRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> DeleteServiceAsync(int serviceId, CancellationToken cancellationToken = default);

    // Orders
    Task<resultBase> AddOrderAsync(int serviceId, string customerId, OrderServiceRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> AcceptOrderAsync(int orderId, string freelancerId, CancellationToken cancellationToken = default);
    Task<resultBase> RejectOrderAsync(int orderId, string freelancerId, CancellationToken cancellationToken = default);
    Task<resultBase> PayOrderAsync(int orderId, CancellationToken cancellationToken = default);
    
    // Webhooks
    Task<resultBase> PaymentSuccessJobOrder(WebHookModel model, CancellationToken cancellationToken = default);
    Task<resultBase> PaymentFailureJobOrder(CancelTransactionModel model, CancellationToken cancellationToken = default);

    // Order Retrieval
    Task<resultBase> GetOrderAsync(int orderId, string userId, CancellationToken cancellationToken = default);
    Task<resultBase> GetOrdersAsync(string userId, CancellationToken cancellationToken = default);

    // Interaction & Lifecycle
    Task<resultBase> SubmitWorkAndMessage(int orderId, string userId, SubmitWorkAndMessageRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> GetConversations(string userId, CancellationToken cancellationToken = default);
    Task<resultBase> GetConversationMessages(int orderId, string userId, CancellationToken cancellationToken = default);
    
    Task<resultBase> CompleteOrderAsync(int orderId, ReviewRequest request, CancellationToken cancellationToken = default);
    Task<resultBase> CancelOrderAsync(int orderId, string userId, CancellationToken cancellationToken = default);
    Task<resultBase> OpenDispute(int orderId, string raiserId, string reasonDetails, CancellationToken cancellationToken = default);
}
