using Khdamatk.Server.Contracts.Conversations;
using Khdamatk.Server.Contracts.Service;
using Khdamatk.Server.Contracts.WebHook;

namespace Khdamatk.Server.Services.Implementations;

public class ServiceOrderService(Database db) : IServiceOrderService
{
    private readonly Database db = db;



    #region CRUD OPERATIONS FOR SERVICES

    public async Task<resultBase> AddServiceAsync(AddServiceRequest request, CancellationToken cancellationToken = default)
    {
        /*TODOs:
         * 
         */
        return Failure(StatusCodes.Status501NotImplemented,FailureMessages.NotImplemented.Title,FailureMessages.NotImplemented.Message);
    }

    public async Task<resultBase> GetServiceAsync(int serviceId, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }

    public async Task<resultBase> GetServicesAsync(CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> UpdateServiceAsync(int serviceId, AddServiceRequest request, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> DeleteServiceAsync(int serviceId, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }


    #endregion



    #region Iniatal Order Operations

    public async Task<resultBase> AddOrderAsync(int ServiceId, string CustomerId, OrderServiceRequest request, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> FreeLancerAcceptOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> FreelancerRejectOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> PayOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        /*TODOs:
         * 
         */

        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }

    public async Task<resultBase> PaymentSuccessJobOrder(WebHookModel model, CancellationToken cancellationToken)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    public async Task<resultBase> PaymentFailureJobOrder(CancelTransactionModel model, CancellationToken cancellationToken)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }


    #endregion



    #region Core Order Operations

    public async Task<resultBase> GetOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> GetOrdersAsync(CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> SubmitWorkAndMessage(int orderId, string userId, SubmitWorkAndMessageRequest request, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }

    public async Task<resultBase> GetConversations(string userId, CancellationToken cancellationToken = default)
    {
        var conversations = await db.ServiceOrders.Where(c => c.CustomerId == userId || c.ServiceProviderId == userId).
            Select(c => new ConversationsSummaryResponse(
                userId,
        (userId == c.CustomerId) ? c.Customer.UserName : c.ServiceProviderProfile.User.UserName,
        (userId == c.CustomerId) ? c.Customer.ProfilePicture.FullPath : c.ServiceProviderProfile.User.ProfilePicture.FullPath,
        c.Service.Title,
        c.Conversation.Messages.OrderByDescending(m => m.Createdat).FirstOrDefault() != null ? c.Conversation.Messages.OrderByDescending(m => m.Createdat).FirstOrDefault()!.Content : "",
                c.Conversation.Messages.OrderByDescending(m => m.Createdat).FirstOrDefault() != null ? c.Conversation.Messages.OrderByDescending(m => m.Createdat).FirstOrDefault()!.Createdat : DateTime.MinValue,
                c.Conversation.Messages.OrderByDescending(m => m.Createdat).FirstOrDefault() != null ? c.Conversation.Messages.OrderByDescending(m => m.Createdat).FirstOrDefault()!.IsRead : true
                 )
                ).ToListAsync(cancellationToken: cancellationToken);


        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message, conversations);
    }
    public async Task<resultBase> GetConversationMessages(int orderId, string UserId, CancellationToken cancellationToken = default)
    {
        var order = await db.ServiceOrders.FirstOrDefaultAsync(o => o.Id == orderId && (o.CustomerId == UserId || o.ServiceProviderId == UserId), cancellationToken: cancellationToken);

        var converationDetailed = new ConversationsDetailed(
            order.Conversation.Id,
            order.Service.Title,
            UserId,
            (UserId == order.CustomerId) ? order.Customer.UserName : order.ServiceProviderProfile.User.UserName,
            (UserId == order.CustomerId) ? order.Customer.ProfilePicture.FullPath : order.ServiceProviderProfile.User.ProfilePicture.FullPath,
            (UserId != order.CustomerId) ? order.CustomerId : order.ServiceProviderId,
            (UserId != order.CustomerId) ? order.Customer.UserName : order.ServiceProviderProfile.User.UserName,
            (UserId != order.CustomerId) ? order.Customer.ProfilePicture.FullPath : order.ServiceProviderProfile.User.ProfilePicture.FullPath,
            order.Conversation.Messages.Select(m => new ConversationMessageResponse(m.Id, m.Content, m.SenderId, m.Createdat)).ToList()
            );


        return Success(StatusCodes.Status200OK, SuccessMessages.General.Title, SuccessMessages.General.Message, converationDetailed);
    }

    #endregion


    #region Final Order Operations

    public async Task<resultBase> CompleteOrderAsync(int orderId, ReviewRequest request, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> CancelOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }
    
    public async Task<resultBase> AriseDisputeAsync(int orderId, string reason, CancellationToken cancellationToken = default)
    {
        return Failure(StatusCodes.Status501NotImplemented, FailureMessages.NotImplemented.Title, FailureMessages.NotImplemented.Message);
    }

    #endregion
}
